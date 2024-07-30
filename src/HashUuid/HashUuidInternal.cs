// /*
//  * Licensed under the Apache License, Version 2.0 (the "License")
//  * you may not use this file except in compliance with the License.
//  * You may obtain a copy of the License at
//  *
//  *   http://www.apache.org/licenses/LICENSE-2.0
//  *
//  * Unless required by applicable law or agreed to in writing, software
//  * distributed under the License is distributed on an "AS IS" BASIS,
//  * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  * See the License for the specific language governing permissions and
//  * limitations under the License.
//  */

using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace System;

internal static class HashUuidInternal
{
	internal interface IVersion
	{
#pragma warning disable CA2252
		static abstract int HashSizeInBytes {
			get;
		}

		static abstract byte Version {
			get;
		}

		static abstract void HashData(ReadOnlySpan<byte> source, Span<byte> destination);

		static abstract HashAlgorithm CreateHashAlgorithm();
#pragma warning restore CA2252
	}

	// ReSharper disable once InconsistentNaming
	// ReSharper disable once ClassNeverInstantiated.Global
	internal sealed class V3 : HashVersion<V3>, IVersion
	{
		public static byte Version => 3;

		public static int HashSizeInBytes {
			get {
#if NET7_0_OR_GREATER
				return MD5.HashSizeInBytes;
#else
				return 16;
#endif
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void HashData(ReadOnlySpan<byte> source, Span<byte> destination) => MD5.HashData(source, destination);

		public static HashAlgorithm CreateHashAlgorithm() => MD5.Create();
	}

	// ReSharper disable once InconsistentNaming
	// ReSharper disable once ClassNeverInstantiated.Global
	internal sealed class V5 : HashVersion<V5>, IVersion
	{
		public static byte Version => 5;

		public static int HashSizeInBytes {
			get {
#if NET7_0_OR_GREATER
				return SHA1.HashSizeInBytes;
#else
				return 20;
#endif
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void HashData(ReadOnlySpan<byte> source, Span<byte> destination) => SHA1.HashData(source, destination);

		public static HashAlgorithm CreateHashAlgorithm() => SHA1.Create();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void SwapByteOrder(ref Span<byte> guid) {
		(
			guid[0], guid[3],
			guid[1], guid[2],
			guid[4], guid[5],
			guid[6], guid[7]
		) = (
			guid[3], guid[0],
			guid[2], guid[1],
			guid[5], guid[4],
			guid[7], guid[6]
		);
	}

	internal class HashVersion<TVersion>
		where TVersion : IVersion
	{
		private const int StackAllocDataThreshold = 512;

		public static void Create(in ReadOnlySpan<byte> namespaceBytes, bool value, in Span<byte> outputBytes) {
			var data = (Span<byte>)stackalloc byte[sizeof(bool)];
			BitConverter.TryWriteBytes(data, value);
			CreateWithStackAllocBuffer(namespaceBytes, data, outputBytes);
		}

		public static void Create(in ReadOnlySpan<byte> namespaceBytes, long value, in Span<byte> outputBytes) {
			var data = (Span<byte>)stackalloc byte[sizeof(long)];
			BitConverter.TryWriteBytes(data, value);
			CreateWithStackAllocBuffer(namespaceBytes, data, outputBytes);
		}

		public static void Create(in ReadOnlySpan<byte> namespaceBytes, double data, in Span<byte> outputBytes) {
			Create(namespaceBytes, Convert.ToDecimal(data), outputBytes);
		}

		public static void Create(in ReadOnlySpan<byte> namespaceBytes, decimal value, in Span<byte> outputBytes) {
			var data = (Span<byte>)stackalloc byte[sizeof(int) * 4];
			decimal.GetBits(value, MemoryMarshal.Cast<byte, int>(data));
			CreateWithStackAllocBuffer(namespaceBytes, data, outputBytes);
		}

#if NET7_0_OR_GREATER
		public static void Create(in ReadOnlySpan<byte> namespaceBytes, Int128 value, in Span<byte> outputBytes) {
			var data = (Span<byte>)stackalloc byte[sizeof(ulong) * 2];
			WriteInteger(value, data);
			CreateWithStackAllocBuffer(namespaceBytes, data, outputBytes);
		}

		public static void Create(in ReadOnlySpan<byte> namespaceBytes, UInt128 value, in Span<byte> outputBytes) {
			var data = (Span<byte>)stackalloc byte[sizeof(ulong) * 2];
			WriteInteger(value, data);
			CreateWithStackAllocBuffer(namespaceBytes, data, outputBytes);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void WriteInteger<TInteger>(TInteger value, in Span<byte> output)
			where TInteger : Numerics.IBinaryInteger<TInteger> =>
			value.TryWriteLittleEndian(output, out _);
#endif

		public static void Create(in ReadOnlySpan<byte> namespaceBytes, DateTimeOffset value, in Span<byte> outputBytes) {
			Create(namespaceBytes, value.ToUnixTimeSeconds(), outputBytes);
		}

		public static void Create(in ReadOnlySpan<byte> namespaceBytes, Guid value, in Span<byte> outputBytes) {
			var data = (Span<byte>)stackalloc byte[16];
#if NET8_0_OR_GREATER
			value.TryWriteBytes(data, false, out _);
#else
			value.TryWriteBytes(data);
			if (!BitConverter.IsLittleEndian) {
				HashUuidInternal.SwapByteOrder(ref data);
			}
#endif
			CreateWithStackAllocBuffer(namespaceBytes, data, outputBytes);
		}

		private static void CreateWithStackAllocBuffer(in ReadOnlySpan<byte> namespaceBytes, in ReadOnlySpan<byte> data, in Span<byte> outputBytes) {
			var buffer = (Span<byte>)stackalloc byte[16 + data.Length];
			Create(namespaceBytes, data, outputBytes, buffer);
		}

		public static void Create(in ReadOnlySpan<byte> namespaceBytes, string data, in Span<byte> outputBytes) {
			Create(namespaceBytes, data, Encoding.UTF8, outputBytes);
		}

		public static void Create(in ReadOnlySpan<byte> namespaceBytes, string data, Encoding encoding, in Span<byte> outputBytes) {
			ArgumentNullException.ThrowIfNull(data);
			ArgumentNullException.ThrowIfNull(encoding);

			var dataSize = encoding.GetByteCount(data);
			if (dataSize > StackAllocDataThreshold) {
				var dataBytes = ArrayPool<byte>.Shared.Rent(dataSize);
				try {
					encoding.GetBytes(data.AsSpan(), dataBytes.AsSpan());
					CreateWithArrayPoolBuffer(namespaceBytes, dataBytes.AsSpan().Slice(0, dataSize), outputBytes);
				} finally {
					ArrayPool<byte>.Shared.Return(dataBytes);
				}
			} else {
				var dataBytes = (Span<byte>)stackalloc byte[dataSize];
				encoding.GetBytes(data.AsSpan(), dataBytes);
				CreateWithStackAllocBuffer(namespaceBytes, dataBytes, outputBytes);
			}
		}

		public static void Create(in ReadOnlySpan<byte> namespaceBytes, byte[] data, in Span<byte> outputBytes) {
			Create(namespaceBytes, data.AsSpan(), outputBytes);
		}

		public static void Create(in ReadOnlySpan<byte> namespaceBytes, in ReadOnlySpan<byte> data, in Span<byte> outputBytes) {
			if (data.Length > StackAllocDataThreshold) {
				CreateWithArrayPoolBuffer(namespaceBytes, data, outputBytes);
			} else {
				CreateWithStackAllocBuffer(namespaceBytes, data, outputBytes);
			}
		}

		public static void Create(in ReadOnlySpan<byte> namespaceBytes, Stream data, in Span<byte> outputBytes) {
			using var hashAlgorithm = TVersion.CreateHashAlgorithm();
			var hash = hashAlgorithm.ComputeHash(data);
			CreateFromHash(hash, outputBytes);
		}

		public static void Create<T>(in ReadOnlySpan<byte> namespaceBytes, T data, JsonSerializerOptions? options, in Span<byte> outputBytes) {
			using var stream = new MemoryStream();
			JsonSerializer.Serialize(stream, data, options);
			stream.Position = 0;
			Create(namespaceBytes, stream, outputBytes);
		}

		public static void Create(in ReadOnlySpan<byte> namespaceBytes, JsonNode data, JsonWriterOptions options, in Span<byte> outputBytes) {
			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream, options);
			data.WriteTo(writer);
			stream.Position = 0;
			Create(namespaceBytes, stream, outputBytes);
		}

		private static void CreateWithArrayPoolBuffer(in ReadOnlySpan<byte> namespaceBytes, in ReadOnlySpan<byte> data, in Span<byte> outputBytes) {
			var buffer = ArrayPool<byte>.Shared.Rent(16 + data.Length);
			try {
				var bufferSpan = buffer.AsSpan()[..(16 + data.Length)];
				Create(namespaceBytes, data, outputBytes, bufferSpan);
			} finally {
				ArrayPool<byte>.Shared.Return(buffer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Create(in ReadOnlySpan<byte> namespaceBytes, in ReadOnlySpan<byte> data, in Span<byte> outputBytes, in Span<byte> buffer) {
			var namespaceSpan = buffer[..16];
			namespaceBytes.CopyTo(namespaceSpan);
			data.CopyTo(buffer[16..]);

			var hash = (Span<byte>)stackalloc byte[TVersion.HashSizeInBytes];
			TVersion.HashData(buffer[..(16 + data.Length)], hash);
			CreateFromHash(hash, outputBytes);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void CreateFromHash(in ReadOnlySpan<byte> hash, in Span<byte> outputBytes) {
			// most bytes from the hash are copied straight to the bytes of the new GUID (steps 5-7, 9, 11-12)
			hash[..16].CopyTo(outputBytes);
			outputBytes[6] = (byte)((outputBytes[6] & 0x0f) | ((TVersion.Version & 0xf) << 4));
			outputBytes[8] = (byte)((outputBytes[8] & 0x3f) | 0x80);
		}
	}
}
