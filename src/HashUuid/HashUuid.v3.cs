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

public static partial class HashUuid
{
	public static Guid CreateV3(this Guid namespaceId, bool data) {
		var output = (Span<byte>)stackalloc byte[16];
		V3.Create(GetNamespace(ref namespaceId), data, output);
		return ToGuid(ref output);
	}

	public static Guid CreateV3(this Guid namespaceId, long data) {
		var output = (Span<byte>)stackalloc byte[16];
		V3.Create(GetNamespace(ref namespaceId), data, output);
		return ToGuid(ref output);
	}

	public static Guid CreateV3(this Guid namespaceId, double data) {
		var output = (Span<byte>)stackalloc byte[16];
		V3.Create(GetNamespace(ref namespaceId), data, output);
		return ToGuid(ref output);
	}

	public static Guid CreateV3(this Guid namespaceId, decimal data) {
		var output = (Span<byte>)stackalloc byte[16];
		V3.Create(GetNamespace(ref namespaceId), data, output);
		return ToGuid(ref output);
	}

#if NET7_0_OR_GREATER
	public static Guid CreateV3(this Guid namespaceId, Int128 data) {
		var output = (Span<byte>)stackalloc byte[16];
		V3.Create(GetNamespace(ref namespaceId), data, output);
		return ToGuid(ref output);
	}

	public static Guid CreateV3(this Guid namespaceId, UInt128 data) {
		var output = (Span<byte>)stackalloc byte[16];
		V3.Create(GetNamespace(ref namespaceId), data, output);
		return ToGuid(ref output);
	}
#endif

	public static Guid CreateV3(this Guid namespaceId, DateTimeOffset data) {
		var output = (Span<byte>)stackalloc byte[16];
		V3.Create(GetNamespace(ref namespaceId), data, output);
		return ToGuid(ref output);
	}

	public static unsafe Guid CreateV3(this Guid namespaceId, Guid data) {
		var output = (Span<byte>)stackalloc byte[16];
		V3.Create(GetNamespace(ref namespaceId), data, output);
		return ToGuid(ref output);
	}

	public static Guid CreateV3(this Guid namespaceId, string data) {
		var output = (Span<byte>)stackalloc byte[16];
		V3.Create(GetNamespace(ref namespaceId), data, output);
		return ToGuid(ref output);
	}

	public static Guid CreateV3(this Guid namespaceId, string data, Encoding encoding) {
		var output = (Span<byte>)stackalloc byte[16];
		V3.Create(GetNamespace(ref namespaceId), data, encoding, output);
		return ToGuid(ref output);
	}

	public static Guid CreateV3(this Guid namespaceId, byte[] data) {
		var output = (Span<byte>)stackalloc byte[16];
		V3.Create(GetNamespace(ref namespaceId), data, output);
		return ToGuid(ref output);
	}

	public static Guid CreateV3(this Guid namespaceId, in ReadOnlySpan<byte> data) {
		var output = (Span<byte>)stackalloc byte[16];
		V3.Create(GetNamespace(ref namespaceId), data, output);
		return ToGuid(ref output);
	}

	public static Guid CreateV3(this Guid namespaceId, Stream data) {
		var output = (Span<byte>)stackalloc byte[16];
		V3.Create(GetNamespace(ref namespaceId), data, output);
		return ToGuid(ref output);
	}

	public static Guid CreateV3FromJson<T>(this Guid namespaceId, T data, JsonSerializerOptions? options = null) {
		var output = (Span<byte>)stackalloc byte[16];
		V3.Create(GetNamespace(ref namespaceId), data, options, output);
		return ToGuid(ref output);
	}

	public static Guid CreateV3FromJson(this Guid namespaceId, JsonNode data, JsonWriterOptions options = default) {
		var output = (Span<byte>)stackalloc byte[16];
		V3.Create(GetNamespace(ref namespaceId), data, options, output);
		return ToGuid(ref output);
	}
}
