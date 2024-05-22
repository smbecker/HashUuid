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

using System.Runtime.CompilerServices;

namespace System;

public static partial class HashUuid
{
	public static readonly Guid NamespaceDns = new("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
	public static readonly Guid NamespaceUrl = new("6ba7b811-9dad-11d1-80b4-00c04fd430c8");
	public static readonly Guid NamespaceOid = new("6ba7b812-9dad-11d1-80b4-00c04fd430c8");
	public static readonly Guid NamespaceX500 = new("6ba7b814-9dad-11d1-80b4-00c04fd430c8");

	private static unsafe ReadOnlySpan<byte> GetNamespace(ref Guid value) {
		var result = new Span<byte>(Unsafe.AsPointer(ref value), 16);
		if (BitConverter.IsLittleEndian) {
			HashUuidInternal.SwapByteOrder(ref result);
		}
		return result;
	}

	private static Guid ToGuid(ref Span<byte> value) {
#if NET8_0_OR_GREATER
		return new Guid(value, true);
#else
		if (BitConverter.IsLittleEndian) {
			HashUuidInternal.SwapByteOrder(ref value);
		}
		return new Guid(value);
#endif
	}
}
