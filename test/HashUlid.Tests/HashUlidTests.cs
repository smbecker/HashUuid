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

using System.Text.Json.Serialization;

namespace System;

public class HashUlidTests
{
	private static readonly Ulid NamespaceDns = new(new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"));

	// Test cases taken from https://github.com/google/uuid repository

	[Fact]
	public void can_generate_uuid_v3() {
		var namespaceId = new Ulid(new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"));
		var expected = new Ulid(new Guid("6fa459ea-ee8a-3ca4-894e-db77e160355e"));
		var actual = namespaceId.CreateV3("python.org");
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void can_generate_uuid_v5() {
		var namespaceId = new Ulid(new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"));
		var expected = new Ulid(new Guid("886313e1-3b8a-5372-9b90-0c9aee199e5d"));
		var actual = namespaceId.CreateV5("python.org");
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void json_type_info_overload_matches_reflection_v3() {
		var data = new JsonTestData("python.org", 42);
		var expected = NamespaceDns.CreateV3FromJson(data, options: null);
		var actual = NamespaceDns.CreateV3FromJson(data, JsonTestContext.Default.JsonTestData);
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void json_type_info_overload_matches_reflection_v5() {
		var data = new JsonTestData("python.org", 42);
		var expected = NamespaceDns.CreateV5FromJson(data, options: null);
		var actual = NamespaceDns.CreateV5FromJson(data, JsonTestContext.Default.JsonTestData);
		Assert.Equal(expected, actual);
	}
}

public sealed record JsonTestData(string Name, int Value);

[JsonSerializable(typeof(JsonTestData))]
internal sealed partial class JsonTestContext : JsonSerializerContext;
