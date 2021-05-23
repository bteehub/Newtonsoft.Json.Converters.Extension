using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Newtonsoft.Json.Converters.Extension.Test
{
    public class UnixDateTimeConverterMillisecondsTest
    {
        [Fact]
        public void SerializeDateTime()
        {
            DateTime unixEpoch = UnixDateTimeConverterMilliseconds.UnixEpoch;

            string result = JsonConvert.SerializeObject(unixEpoch, new UnixDateTimeConverterMilliseconds());

            Assert.Equal("0", result);
        }

        [Fact]
        public void SerializeDateTimeNow()
        {
            DateTime unixEpoch = UnixDateTimeConverterMilliseconds.UnixEpoch;

            DateTime now = DateTime.Now;
            long nowMilliseconds = (long)(now.ToUniversalTime() - unixEpoch).TotalMilliseconds;

            string result = JsonConvert.SerializeObject(now, new UnixDateTimeConverterMilliseconds());

            Assert.Equal($"{nowMilliseconds}", result);
        }

        [Fact]
        public void SerializeInvalidDate()
        {
            DateTime unixEpoch = UnixDateTimeConverterMilliseconds.UnixEpoch;

            var exception = Assert.Throws<JsonSerializationException>(() => JsonConvert.SerializeObject(unixEpoch.AddMilliseconds(-1), new UnixDateTimeConverterMilliseconds()));
            Assert.Equal("Cannot convert date value that is before Unix epoch of 00:00:00 UTC on 1 January 1970.", exception.Message);
        }

        [Fact]
        public void WriteJsonInvalidType()
        {
            UnixDateTimeConverterMilliseconds converter = new UnixDateTimeConverterMilliseconds();

            var exception = Assert.Throws<JsonSerializationException>(() => converter.WriteJson(new JTokenWriter(), new object(), new JsonSerializer()));
            Assert.Equal("Expected date object value.", exception.Message);
        }

        [Fact]
        public void SerializeDateTimeOffset()
        {
            DateTimeOffset now = new DateTimeOffset(2018, 1, 1, 16, 1, 16, 155, TimeSpan.FromHours(-5));

            string result = JsonConvert.SerializeObject(now, new UnixDateTimeConverterMilliseconds());

            Assert.Equal("1514840476155", result);
        }

        [Fact]
        public void SerializeNullableDateTimeClass()
        {
            DateTime? t = null;

            UnixDateTimeConverterMilliseconds converter = new UnixDateTimeConverterMilliseconds();

            string result = JsonConvert.SerializeObject(t, converter);

            Assert.Equal("null", result);
        }

        [Fact]
        public void SerializeNullableDateTimeOffsetClass()
        {
            DateTimeOffset? t = null;

            UnixDateTimeConverterMilliseconds converter = new UnixDateTimeConverterMilliseconds();

            string result = JsonConvert.SerializeObject(t, converter);

            Assert.Equal("null", result);
        }

        [Fact]
        public void DeserializeDateTimeOffset()
        {
            UnixDateTimeConverterMilliseconds converter = new UnixDateTimeConverterMilliseconds();
            DateTimeOffset d = new DateTimeOffset(1970, 2, 1, 20, 6, 18, 145, TimeSpan.Zero);

            string json = JsonConvert.SerializeObject(d, converter);

            DateTimeOffset result = JsonConvert.DeserializeObject<DateTimeOffset>(json, converter);

            Assert.Equal(new DateTimeOffset(1970, 2, 1, 20, 6, 18, 145, TimeSpan.Zero), result);
        }

        [Fact]
        public void DeserializeStringToDateTimeOffset()
        {
            DateTimeOffset result = JsonConvert.DeserializeObject<DateTimeOffset>(@"""1514840476147""", new UnixDateTimeConverterMilliseconds());

            Assert.Equal(new DateTimeOffset(2018, 1, 1, 21, 1, 16, 147, TimeSpan.Zero), result);
        }

        [Fact]
        public void DeserializeInvalidStringToDateTimeOffset()
        {
            var exception = Assert.Throws<JsonSerializationException>(() => JsonConvert.DeserializeObject<DateTimeOffset>(@"""PIE""", new UnixDateTimeConverterMilliseconds()));

            Assert.Equal("Cannot convert invalid value to System.DateTimeOffset.", exception.Message);
        }

        [Fact]
        public void DeserializeIntegerToDateTime()
        {
            DateTime result = JsonConvert.DeserializeObject<DateTime>("1514840476159", new UnixDateTimeConverterMilliseconds());

            Assert.Equal(new DateTime(2018, 1, 1, 21, 1, 16, 159, DateTimeKind.Utc), result);
        }

        [Fact]
        public void DeserializeNullToNullable()
        {
            DateTime? result = JsonConvert.DeserializeObject<DateTime?>("null", new UnixDateTimeConverterMilliseconds());

            Assert.Null(result);
        }

        [Fact]
        public void DeserializeInvalidValue()
        {
            var exception = Assert.Throws<JsonSerializationException>(() => JsonConvert.DeserializeObject<DateTime>("-1", new UnixDateTimeConverterMilliseconds()));

            Assert.Equal("Cannot convert value that is before Unix epoch of 00:00:00 UTC on 1 January 1970 to System.DateTime.", exception.Message);
        }

        [Fact]
        public void DeserializeInvalidValueType()
        {
            var exception = Assert.Throws<JsonSerializationException>(() => JsonConvert.DeserializeObject<DateTime>("false", new UnixDateTimeConverterMilliseconds()));

            Assert.Equal("Unexpected token parsing date. Expected Integer or String, got Boolean.", exception.Message);
        }

        [Fact]
        public void ConverterList()
        {
            UnixConverterList<object> l1 = new UnixConverterList<object>
            {
                new DateTime(2018, 1, 1, 21, 1, 16, 149, DateTimeKind.Utc),
                new DateTime(1970, 1, 1, 0, 0, 3, 789, DateTimeKind.Utc),
            };

            string json = JsonConvert.SerializeObject(l1, Formatting.None);
            Assert.Equal("[1514840476149,3789]", json);

            UnixConverterList<object> l2 = JsonConvert.DeserializeObject<UnixConverterList<object>>(json);
            Assert.NotNull(l2);

            Assert.Equal(new DateTime(2018, 1, 1, 21, 1, 16, 149, DateTimeKind.Utc), l2[0]);
            Assert.Equal(new DateTime(1970, 1, 1, 0, 0, 3, 789, DateTimeKind.Utc), l2[1]);
        }

        [Fact]
        public void ConverterDictionary()
        {
            UnixConverterDictionary<object> l1 = new UnixConverterDictionary<object>
            {
                {"First", new DateTime(1970, 1, 1, 0, 0, 3, 754, DateTimeKind.Utc)},
                {"Second", new DateTime(2018, 1, 1, 21, 1, 16, 125, DateTimeKind.Utc)},
            };

            string json = JsonConvert.SerializeObject(l1, Formatting.None);
            Assert.Equal(@"{""First"":3754,""Second"":1514840476125}", json);

            UnixConverterDictionary<object> l2 = JsonConvert.DeserializeObject<UnixConverterDictionary<object>>(json);
            Assert.NotNull(l2);

            Assert.Equal(new DateTime(1970, 1, 1, 0, 0, 3, 754, DateTimeKind.Utc), l2["First"]);
            Assert.Equal(new DateTime(2018, 1, 1, 21, 1, 16, 125, DateTimeKind.Utc), l2["Second"]);
        }

        [Fact]
        public void ConverterObject()
        {
            UnixConverterObject obj1 = new UnixConverterObject
            {
                Object1 = new DateTime(1970, 1, 1, 0, 0, 3, 145, DateTimeKind.Utc),
                Object2 = null,
                ObjectNotHandled = new DateTime(2018, 1, 1, 21, 1, 16, 147, DateTimeKind.Utc)
            };

            string json = JsonConvert.SerializeObject(obj1, Formatting.None);
            Assert.Equal(@"{""Object1"":3145,""Object2"":null,""ObjectNotHandled"":1514840476147}", json);

            UnixConverterObject obj2 = JsonConvert.DeserializeObject<UnixConverterObject>(json);
            Assert.NotNull(obj2);

            Assert.Equal(new DateTime(1970, 1, 1, 0, 0, 3, 145, DateTimeKind.Utc), obj2.Object1);
            Assert.Null(obj2.Object2);
            Assert.Equal(new DateTime(2018, 1, 1, 21, 1, 16, 147, DateTimeKind.Utc), obj2.ObjectNotHandled);
        }
    }


    [JsonArray(ItemConverterType = typeof(UnixDateTimeConverterMilliseconds))]
    public class UnixConverterList<T> : List<T> { }

    [JsonDictionary(ItemConverterType = typeof(UnixDateTimeConverterMilliseconds))]
    public class UnixConverterDictionary<T> : Dictionary<string, T> { }

    [JsonObject(ItemConverterType = typeof(UnixDateTimeConverterMilliseconds))]
    public class UnixConverterObject
    {
        public object Object1 { get; set; }

        public object Object2 { get; set; }

        [JsonConverter(typeof(UnixDateTimeConverterMilliseconds))]
        public object ObjectNotHandled { get; set; }
    }
}
