using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace UlidTests
{
    public class UlidTest
    {
        [Fact]
        public void New_ByteEquals_ToString_Equals()
        {
            for (int i = 0; i < 100; i++)
            {
                {
                    var ulid = Ulid.NewUlid();
                    var nulid = new NUlid.Ulid(ulid.ToByteArray());

                    ulid.ToByteArray().Should().BeEquivalentTo(nulid.ToByteArray());
                    ulid.ToString().Should().Be(nulid.ToString());
                    ulid.Equals(ulid).Should().BeTrue();
                    ulid.Equals(Ulid.NewUlid()).Should().BeFalse();
                }
                {
                    var nulid = NUlid.Ulid.NewUlid();
                    var ulid = new Ulid(nulid.ToByteArray());

                    ulid.ToByteArray().Should().BeEquivalentTo(nulid.ToByteArray());
                    ulid.ToString().Should().Be(nulid.ToString());
                    ulid.Equals(ulid).Should().BeTrue();
                    ulid.Equals(Ulid.NewUlid()).Should().BeFalse();
                }
            }
        }

        [Fact]
        public void Compare_Time()
        {
            var times = new DateTimeOffset[]
            {
                new DateTime(2012,12,4),
                new DateTime(2011,12,31),
                new DateTime(2012,1,5),
                new DateTime(2013,12,4),
                new DateTime(2016,12,4),
            };

            times.Select(x => Ulid.NewUlid(x)).OrderBy(x => x).Select(x => x.Time).Should().BeEquivalentTo(times.OrderBy(x => x));
        }

        [Fact]
        public void Parse()
        {
            for (int i = 0; i < 100; i++)
            {
                var nulid = NUlid.Ulid.NewUlid();
                Ulid.Parse(nulid.ToString()).ToByteArray().Should().BeEquivalentTo(nulid.ToByteArray());
            }
        }
        [Fact]
        public void Randomness()
        {
            var d = DateTime.Parse("1970/1/1 00:00:00Z");
            var r = new byte[10];
            var first = Ulid.NewUlid(d, r);
            var second = Ulid.NewUlid(d, r);
            first.ToString().Should().BeEquivalentTo(second.ToString());
            // Console.WriteLine($"first={first.ToString()}, second={second.ToString()}");
        }

        [Fact]
        public void GuidInterop()
        {
            var ulid = Ulid.NewUlid();
            var guid = ulid.ToGuid();
            var ulid2 = new Ulid(guid);

            ulid2.Should().BeEquivalentTo(ulid, "a Ulid-Guid roundtrip should result in identical values");
        }
    }
}
