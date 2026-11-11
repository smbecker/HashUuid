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

public class HashUuidTests
{
	// Test cases taken from https://github.com/google/uuid repository

	[Fact]
	public void can_generate_uuid_v3() {
		var expected = new Guid("6fa459ea-ee8a-3ca4-894e-db77e160355e");
		var actual = HashUuid.NamespaceDns.CreateV3("python.org");
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void can_generate_uuid_v5() {
		var expected = new Guid("886313e1-3b8a-5372-9b90-0c9aee199e5d");
		var actual = HashUuid.NamespaceDns.CreateV5("python.org");
		Assert.Equal(expected, actual);
	}

	// Expected values computed independently from RFC 4122: MD5/SHA-1 over the namespace bytes followed by
	// the IEEE 754 binary64 value in little-endian order (decimals: decimal.GetBits as little-endian int32s).
	[Theory]
	[InlineData(0.1, "07257cd3-6696-3d45-9f99-54de22ddd88d", "1fb557df-c410-5ab0-b6fa-8740d2bbad4a")]
	[InlineData(1.5, "15a7b5d9-013f-3409-8f44-aec78e58d89c", "e41d45e2-938e-5b9d-b27d-fe4e8fe4bccb")]
	[InlineData(-2.5, "84ae38e2-8da3-3c87-9b63-f49065ee22fe", "7d6a6eff-88a8-5c82-b8c7-68c10ed8f8b6")]
	[InlineData(Math.PI, "b3c8925a-3750-341d-b88e-a2d5d709ba9a", "659c327d-db9c-5cf5-8c36-b4d1427886d6")]
	[InlineData(0.0, "40e46e6b-19f1-3c02-9096-248c62fbe9e6", "7689a587-dc34-5882-9aeb-9368592f805f")]
	[InlineData(double.PositiveInfinity, "2997309f-83ab-31af-a123-1336476bcbc7", "c20499a9-04d2-525c-8932-77363326e29d")]
	[InlineData(double.NaN, "8d5b6fbb-50a4-3f6f-9e78-7393fc471d30", "d189d672-f97b-52e6-9153-520fcf6ff5a6")]
	public void can_generate_uuid_from_double(double data, string expectedV3, string expectedV5) {
		Assert.Equal(new Guid(expectedV3), HashUuid.NamespaceDns.CreateV3(data));
		Assert.Equal(new Guid(expectedV5), HashUuid.NamespaceDns.CreateV5(data));
	}

	[Fact]
	public void negative_zero_produces_the_same_uuid_as_zero() {
		Assert.Equal(HashUuid.NamespaceDns.CreateV3(0.0), HashUuid.NamespaceDns.CreateV3(-0.0));
		Assert.Equal(HashUuid.NamespaceDns.CreateV5(0.0), HashUuid.NamespaceDns.CreateV5(-0.0));
	}

	[Fact]
	public void nan_payloads_produce_the_same_uuid() {
		var otherNaN = BitConverter.Int64BitsToDouble(0x7FF8000000000001);
		Assert.Equal(HashUuid.NamespaceDns.CreateV3(double.NaN), HashUuid.NamespaceDns.CreateV3(otherNaN));
		Assert.Equal(HashUuid.NamespaceDns.CreateV5(double.NaN), HashUuid.NamespaceDns.CreateV5(otherNaN));
	}

	[Theory]
	[InlineData(0L, "40e46e6b-19f1-3c02-9096-248c62fbe9e6", "7689a587-dc34-5882-9aeb-9368592f805f")]
	[InlineData(1L, "22ed33aa-626f-3cdf-9ff5-55df315b1dad", "3f254482-3457-5b56-85e9-ac473ab6ebf4")]
	[InlineData(-1L, "ee36910e-0226-39bf-ba3f-d7aabf5d5364", "d77b9728-d444-51b1-9236-ef477574faf3")]
	[InlineData(1234567890123L, "0ab959d6-3628-302e-a880-4d738fe8fa06", "f90759c6-e6b2-5df3-9dd0-59cf54e102b7")]
	[InlineData(long.MaxValue, "b75bb506-0f8e-3cdd-8ead-d086ef34b3fd", "f36a52d9-dcec-58b0-a7ce-ee566cbda24c")]
	public void can_generate_uuid_from_long(long data, string expectedV3, string expectedV5) {
		Assert.Equal(new Guid(expectedV3), HashUuid.NamespaceDns.CreateV3(data));
		Assert.Equal(new Guid(expectedV5), HashUuid.NamespaceDns.CreateV5(data));
	}

	[Fact]
	public void can_generate_uuid_from_date_time_offset() {
		var data = DateTimeOffset.FromUnixTimeSeconds(1700000000);
		Assert.Equal(new Guid("55ca26f7-a474-39a8-aacc-2eddcbb67a34"), HashUuid.NamespaceDns.CreateV3(data));
		Assert.Equal(new Guid("8f7785a4-f473-5675-bf64-c961e18ce2a7"), HashUuid.NamespaceDns.CreateV5(data));
	}

	[Theory]
	[InlineData("1.5", "b05dc77d-7b95-3636-af89-fffad8caf532", "08babee2-e0af-5c5f-ac7c-cda94d78b5d2")]
	[InlineData("0.1", "642c0d73-0a5e-37b2-bd25-781bdc46008b", "faffed16-c21d-5e23-ad05-3e563631d062")]
	[InlineData("-123.456", "f42f6ba4-b6f9-3f04-95b0-041e3595ee27", "1f99d834-209e-5ad3-aa17-7de91ba92db3")]
	public void can_generate_uuid_from_decimal(string data, string expectedV3, string expectedV5) {
		var value = decimal.Parse(data, Globalization.CultureInfo.InvariantCulture);
		Assert.Equal(new Guid(expectedV3), HashUuid.NamespaceDns.CreateV3(value));
		Assert.Equal(new Guid(expectedV5), HashUuid.NamespaceDns.CreateV5(value));
	}

	[Fact]
	public void json_type_info_overload_matches_reflection_v3() {
		var data = new JsonTestData("python.org", 42);
		var expected = HashUuid.NamespaceDns.CreateV3FromJson(data, options: null);
		var actual = HashUuid.NamespaceDns.CreateV3FromJson(data, JsonTestContext.Default.JsonTestData);
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void json_type_info_overload_matches_reflection_v5() {
		var data = new JsonTestData("python.org", 42);
		var expected = HashUuid.NamespaceDns.CreateV5FromJson(data, options: null);
		var actual = HashUuid.NamespaceDns.CreateV5FromJson(data, JsonTestContext.Default.JsonTestData);
		Assert.Equal(expected, actual);
	}
}

public sealed record JsonTestData(string Name, int Value);

[JsonSerializable(typeof(JsonTestData))]
internal sealed partial class JsonTestContext : JsonSerializerContext;
