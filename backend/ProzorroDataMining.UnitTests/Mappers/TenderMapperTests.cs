namespace ProzorroDataMining.UnitTests.Mappers;

using ProzorroDataMining.Application.Mappers;
using ProzorroDataMining.Contracts.DTOs.Prozorro;
using Shouldly;

public class TenderMapperTests
{
    private static TenderData CreateValidTenderData() => new()
    {
        Id = "abc123",
        Status = "complete",
        DateCreated = new DateTimeOffset(2025, 12, 1, 0, 0, 0, TimeSpan.Zero),
        DateModified = new DateTimeOffset(2025, 12, 15, 0, 0, 0, TimeSpan.Zero),
        Value = new MoneyValue { Amount = 100_000, Currency = "UAH" },
        ProcuringEntity = new ProcuringEntity
        {
            Name = "Тестовий замовник",
            Identifier = new Identifier { Id = "12345678" }
        },
        Items =
        [
            new TenderItemData
            {
                Id = "item1",
                Classification = new Classification { Id = "09310000-5", Description = "Електрична енергія" },
                Description = "Електроенергія",
                Quantity = 1000,
                Unit = new Unit { Name = "кВт/год", Code = "KWH" }
            }
        ],
        Contracts =
        [
            new ContractData
            {
                Id = "contract1",
                Value = new MoneyValue { Amount = 90_000, Currency = "UAH" },
                Status = "active",
                DateSigned = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
            }
        ],
        Awards =
        [
            new AwardData
            {
                Id = "award1",
                Value = new MoneyValue { Amount = 90_000, Currency = "UAH" },
                Status = "active",
                Suppliers =
                [
                    new Supplier
                    {
                        Name = "Тестовий постачальник",
                        Identifier = new Identifier { Id = "87654321" }
                    }
                ]
            }
        ]
    };

    [Fact]
    public void MapToEntity_ValidData_ReturnsTender()
    {
        var data = CreateValidTenderData();

        var result = TenderMapper.MapToEntity(data);

        result.ShouldNotBeNull();
        result!.Id.ShouldBe("abc123");
        result.Status.ShouldBe("complete");
        result.BudgetAmount.ShouldBe(100_000);
        result.ProcuringEntityName.ShouldBe("Тестовий замовник");
    }

    [Fact]
    public void MapToEntity_ValidData_MapsItemsCorrectly()
    {
        var data = CreateValidTenderData();

        var result = TenderMapper.MapToEntity(data);

        result.ShouldNotBeNull();
        result!.Items.Count.ShouldBe(1);
        result.Items.First().CpvCode.ShouldBe("09310000-5");
        result.Items.First().Quantity.ShouldBe(1000);
    }

    [Fact]
    public void MapToEntity_ValidData_MapsContractsCorrectly()
    {
        var data = CreateValidTenderData();

        var result = TenderMapper.MapToEntity(data);

        result.ShouldNotBeNull();
        result!.Contracts.Count.ShouldBe(1);
        result.Contracts.First().ContractValue.ShouldBe(90_000);
        result.Contracts.First().Status.ShouldBe("active");
    }

    [Fact]
    public void MapToEntity_ValidData_MapsAwardsCorrectly()
    {
        var data = CreateValidTenderData();

        var result = TenderMapper.MapToEntity(data);

        result.ShouldNotBeNull();
        result!.Awards.Count.ShouldBe(1);
        result.Awards.First().SupplierName.ShouldBe("Тестовий постачальник");
        result.Awards.First().AwardValue.ShouldBe(90_000);
    }

    [Fact]
    public void MapToEntity_NullId_ReturnsNull()
    {
        var data = CreateValidTenderData();
        data.Id = string.Empty;

        var result = TenderMapper.MapToEntity(data);

        result.ShouldBeNull();
    }

    [Fact]
    public void MapToEntity_NullValue_ReturnsNull()
    {
        var data = CreateValidTenderData();
        data.Value = null;

        var result = TenderMapper.MapToEntity(data);

        result.ShouldBeNull();
    }

    [Fact]
    public void MapToEntity_NullProcuringEntity_ReturnsNull()
    {
        var data = CreateValidTenderData();
        data.ProcuringEntity = null;

        var result = TenderMapper.MapToEntity(data);

        result.ShouldBeNull();
    }

    [Fact]
    public void MapToEntity_NullContracts_MapsWithEmptyContracts()
    {
        var data = CreateValidTenderData();
        data.Contracts = null;

        var result = TenderMapper.MapToEntity(data);

        result.ShouldNotBeNull();
        result!.Contracts.ShouldBeEmpty();
    }

    [Fact]
    public void MapToEntity_NullAwards_MapsWithEmptyAwards()
    {
        var data = CreateValidTenderData();
        data.Awards = null;

        var result = TenderMapper.MapToEntity(data);

        result.ShouldNotBeNull();
        result!.Awards.ShouldBeEmpty();
    }

    [Fact]
    public void MapToEntity_NullItems_MapsWithEmptyItems()
    {
        var data = CreateValidTenderData();
        data.Items = null;

        var result = TenderMapper.MapToEntity(data);

        result.ShouldNotBeNull();
        result!.Items.ShouldBeEmpty();
    }

    [Fact]
    public void MapToEntity_AwardWithNullSuppliers_MapsWithEmptyAwards()
    {
        var data = CreateValidTenderData();
        data.Awards![0].Suppliers = null;

        var result = TenderMapper.MapToEntity(data);

        result.ShouldNotBeNull();
        result!.Awards.ShouldBeEmpty();
    }

    [Fact]
    public void MapToEntity_DateTimeConvertedToUtc()
    {
        var data = CreateValidTenderData();
        data.DateCreated = new DateTimeOffset(2025, 12, 1, 10, 0, 0, TimeSpan.FromHours(2));

        var result = TenderMapper.MapToEntity(data);

        result.ShouldNotBeNull();
        result!.DateCreated.Offset.ShouldBe(TimeSpan.Zero);
    }
}