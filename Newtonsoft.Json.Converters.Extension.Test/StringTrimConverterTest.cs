using System.IO;
using Xunit;

namespace Newtonsoft.Json.Converters.Extension.Test
{
    public class StringTrimConverterTest
    {

        [Fact]
        public void ReadJsonGenericExistingValueNull()
        {
            var stringReader = new StringReader("' String! '");
            var jsonReader = new JsonTextReader(stringReader);
            jsonReader.Read();

            var jsonConverter = new StringTrimConverter();
            var s = jsonConverter.ReadJson(jsonReader, typeof(string), null, false, null);

            Assert.Equal(@"String!", s);
        }

        [Fact]
        public void ReadJsonGenericExistingValueString()
        {
            var stringReader = new StringReader("' String! '");
            var jsonReader = new JsonTextReader(stringReader);
            jsonReader.Read();

            var jsonConverter = new StringTrimConverter();
            var s = jsonConverter.ReadJson(jsonReader, typeof(string), " Existing! ", true, null);

            Assert.Equal(@"String!Existing!", s);
        }

        [Fact]
        public void ReadJsonObjectExistingValueNull()
        {
            var stringReader = new StringReader("' String! '");
            var jsonReader = new JsonTextReader(stringReader);
            jsonReader.Read();

            var jsonConverter = new StringTrimConverter();
            var s = (string)jsonConverter.ReadJson(jsonReader, typeof(string), null, null);

            Assert.Equal(@"String!", s);
        }

        [Fact]
        public void ReadJsonObjectExistingValueWrongType()
        {
            var sr = new StringReader("' String! '");
            var jsonReader = new JsonTextReader(sr);
            jsonReader.Read();

            var jsonConverter = new StringTrimConverter();

            var exception = Assert.Throws<JsonSerializationException>(() => jsonConverter.ReadJson(jsonReader, typeof(string), 12345, null));
            Assert.Equal("Converter cannot read JSON with the specified existing value. System.String is required.", exception.Message);
        }

        [Fact]
        public void WriteJsonObject()
        {
            var stringWriter = new StringWriter();
            var jsonWriter = new JsonTextWriter(stringWriter);

            var jsonConverter = new StringTrimConverter();
            jsonConverter.WriteJson(jsonWriter, (object)" String! ", null);

            Assert.Equal(@"""String!""", stringWriter.ToString());
        }

        [Fact]
        public void WriteJsonGeneric()
        {
            var stringWriter = new StringWriter();
            var jsonWriter = new JsonTextWriter(stringWriter);

            var jsonConverter = new StringTrimConverter();
            jsonConverter.WriteJson(jsonWriter, " String! ", null);

            Assert.Equal(@"""String!""", stringWriter.ToString());
        }

        [Fact]
        public void WriteJsonBadType()
        {
            var stringWriter = new StringWriter();
            var jsonWriter = new JsonTextWriter(stringWriter);

            var jsonConverter = new StringTrimConverter();

            var exception = Assert.Throws<JsonSerializationException>(() => jsonConverter.WriteJson(jsonWriter, 123, null));
            Assert.Equal("Converter cannot write specified value to JSON. System.String is required.", exception.Message);
        }
    }
}
