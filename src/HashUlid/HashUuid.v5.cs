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

using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.HashUuidInternal;

namespace System;

public static partial class HashUlid
{
	public static Ulid CreateV5(this Ulid namespaceId, bool data) {
		var output = (Span<byte>)stackalloc byte[16];
		V5.Create(GetNamespace(ref namespaceId), data, output);
		return new(output);
	}

	public static Ulid CreateV5(this Ulid namespaceId, long data) {
		var output = (Span<byte>)stackalloc byte[16];
		V5.Create(GetNamespace(ref namespaceId), data, output);
		return new(output);
	}

	public static Ulid CreateV5(this Ulid namespaceId, double data) {
		var output = (Span<byte>)stackalloc byte[16];
		V5.Create(GetNamespace(ref namespaceId), data, output);
		return new(output);
	}

	public static Ulid CreateV5(this Ulid namespaceId, decimal data) {
		var output = (Span<byte>)stackalloc byte[16];
		V5.Create(GetNamespace(ref namespaceId), data, output);
		return new(output);
	}

	public static Ulid CreateV5(this Ulid namespaceId, Int128 data) {
		var output = (Span<byte>)stackalloc byte[16];
		V5.Create(GetNamespace(ref namespaceId), data, output);
		return new(output);
	}

	public static Ulid CreateV5(this Ulid namespaceId, UInt128 data) {
		var output = (Span<byte>)stackalloc byte[16];
		V5.Create(GetNamespace(ref namespaceId), data, output);
		return new(output);
	}

	public static Ulid CreateV5(this Ulid namespaceId, DateTimeOffset data) {
		var output = (Span<byte>)stackalloc byte[16];
		V5.Create(GetNamespace(ref namespaceId), data, output);
		return new(output);
	}

	public static unsafe Ulid CreateV5(this Ulid namespaceId, Guid data) {
		var output = (Span<byte>)stackalloc byte[16];
		V5.Create(GetNamespace(ref namespaceId), data, output);
		return new(output);
	}

	public static Ulid CreateV5(this Ulid namespaceId, string data) {
		var output = (Span<byte>)stackalloc byte[16];
		V5.Create(GetNamespace(ref namespaceId), data, output);
		return new(output);
	}

	public static Ulid CreateV5(this Ulid namespaceId, string data, Encoding encoding) {
		var output = (Span<byte>)stackalloc byte[16];
		V5.Create(GetNamespace(ref namespaceId), data, encoding, output);
		return new(output);
	}

	public static Ulid CreateV5(this Ulid namespaceId, byte[] data) {
		var output = (Span<byte>)stackalloc byte[16];
		V5.Create(GetNamespace(ref namespaceId), data, output);
		return new(output);
	}

	public static Ulid CreateV5(this Ulid namespaceId, in ReadOnlySpan<byte> data) {
		var output = (Span<byte>)stackalloc byte[16];
		V5.Create(GetNamespace(ref namespaceId), data, output);
		return new(output);
	}

	public static Ulid CreateV5(this Ulid namespaceId, Stream data) {
		var output = (Span<byte>)stackalloc byte[16];
		V5.Create(GetNamespace(ref namespaceId), data, output);
		return new(output);
	}

	public static Ulid CreateV5FromJson<T>(this Ulid namespaceId, T data, JsonSerializerOptions? options = null) {
		var output = (Span<byte>)stackalloc byte[16];
		V5.Create(GetNamespace(ref namespaceId), data, options, output);
		return new(output);
	}

	public static Ulid CreateV5FromJson(this Ulid namespaceId, JsonNode data, JsonWriterOptions options = default) {
		var output = (Span<byte>)stackalloc byte[16];
		V5.Create(GetNamespace(ref namespaceId), data, options, output);
		return new(output);
	}
}
