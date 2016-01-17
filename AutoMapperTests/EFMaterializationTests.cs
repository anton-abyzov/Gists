using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using AutoMapper;
using NUnit.Framework;
using Assert = Xunit.Assert;

namespace AutoMapperTests
{
    
    public class EFMaterializationTests
    {
        /// <summary>
        /// Domain entity (WcChallengeDomain in our case) should have all properties mentioned on materialization of EF6 and below. In this case optional field Description is missing
        /// on materialization (Select with new WcChallengeDomain) and EF doesn't support it
        /// </summary>
        [Test]
        public void EF6WillThrowExcOnMaterializationOfDomainEntityWithMissingField()
        {
            //arrange
            var context = new SepDbContext();
            Database.SetInitializer<SepDbContext>(null);

            var data = context.Challenges.Select(x => new WcChallengeDomain()
            {
                Id = x.ChallengeId,
                Name = x.Name
            });

            //act
            Mapper.CreateMap<WcChallengeDomain, WcChallengeDto>();
            var mappedToDto = data.Map<WcChallengeDomain, WcChallengeDto>();

            //assert
            Assert.Throws<NotSupportedException>(() => mappedToDto.ToList());
        }

        [Test]
        public void EF6WillMapDomainEntitySuccessfully()
        {
            //arrange
            var context = new SepDbContext();
            Database.SetInitializer<SepDbContext>(null);

            var data = context.Challenges.Select(x => new WcChallengeDomain()
            {
                Id = x.ChallengeId,
                Name = x.Name,
                Description = null // populating default value
            });

            //act
            Mapper.CreateMap<WcChallengeDomain, WcChallengeDto>();
            var mappedToDto = data.Map<WcChallengeDomain, WcChallengeDto>();

            //assert
            Assert.NotNull(mappedToDto.ToList());
        }

        [Test]
        public void TestCasting()
        {
            //arrange
            var context = new SepDbContext();
            Database.SetInitializer<SepDbContext>(null);

            var challengeDalDtos = context.Challenges.Where(x => x.ChallengeId == 100000);// with this condition we guarantee we have empty set
            var data = challengeDalDtos.Select(x => x.ExternalUID); //when selecting on empty set we still get empty set

            //act
            var first = data.FirstOrDefault(); // firstordefault on empty set 

            //assert
            Assert.Equal(first, default(Guid));
        }

        public class SepDbContext : DbContext
        {
            public SepDbContext() : base("SepDB") { }

            public DbSet<WcChallengeDalDto> Challenges { get; set; }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Configurations.Add(new WcChallengeMap());
            }
        }
        public class WcChallengeMap : EntityTypeConfiguration<WcChallengeDalDto>
        {
            public WcChallengeMap(string schema = "dbo")
            {
                ToTable("WCChallenges", schema);
                HasKey(x => x.ChallengeId);
                Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
                Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(800);
                Property(x => x.ExternalUID).HasColumnName("ExternalUID").IsRequired();
            }
        }
        public class WcChallengeDalDto
        {
            public int ChallengeId { get; set; }
            public virtual string Description { get; set; }
            public virtual string Name { get; set; }

            private Guid _externalUID = Guid.NewGuid();
            public virtual Guid ExternalUID {
                get { return _externalUID; }
                set { _externalUID = value; }
            }

        }
        public class WcChallengeDomain
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
        public class WcChallengeDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}