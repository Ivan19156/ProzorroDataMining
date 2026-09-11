namespace ProzorroDataMining.UnitTests.Services;

using Microsoft.Extensions.Logging;
using NSubstitute;
using ProzorroDataMining.Application.Http;
using ProzorroDataMining.Application.Services;
using ProzorroDataMining.Contracts.DTOs.Prozorro;
using ProzorroDataMining.Domain.Entities;
using ProzorroDataMining.Domain.Interfaces;
using Shouldly;

public class TenderIngestionServiceTests
{
    private readonly IProzorroClient _prozorroClient = Substitute.For<IProzorroClient>();
    private readonly ITenderRepository _repository = Substitute.For<ITenderRepository>();
    private readonly ILogger<TenderIngestionService> _logger = Substitute.For<ILogger<TenderIngestionService>>();
    private readonly TenderIngestionService _sut;

    public TenderIngestionServiceTests()
    {
        _sut = new TenderIngestionService(_prozorroClient, _repository, _logger);
    }

    private static TenderDetailResponse CreateDetailResponse(
        string status = "complete",
        string cpvCode = "09310000-5") => new()
        {
            Data = new TenderData
            {
                Id = "tender1",
                Status = status,
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
                    Classification = new Classification { Id = cpvCode },
                }
            ]
            }
        };

    private static TenderListResponse CreateListResponse(string tenderId = "tender1") => new()
    {
        Data =
        [
            new TenderListItem
            {
                Id = tenderId,
                DateModified = new DateTimeOffset(2025, 12, 15, 0, 0, 0, TimeSpan.Zero)
            }
        ],
        NextPage = null
    };

    [Fact]
    public async Task RunAsync_ValidTender_UpsertsCalled()
    {
        _prozorroClient
            .GetTenderListAsync(Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns(CreateListResponse());

        _prozorroClient
            .GetTenderDetailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(CreateDetailResponse());

        await _sut.RunAsync();

        await _repository.Received(1).UpsertAsync(
            Arg.Any<Tender>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RunAsync_WrongStatus_UpsertNotCalled()
    {
        _prozorroClient
            .GetTenderListAsync(Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns(CreateListResponse());

        _prozorroClient
            .GetTenderDetailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(CreateDetailResponse(status: "active"));

        await _sut.RunAsync();

        await _repository.DidNotReceive().UpsertAsync(
            Arg.Any<Tender>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RunAsync_WrongCpvCode_UpsertNotCalled()
    {
        _prozorroClient
            .GetTenderListAsync(Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns(CreateListResponse());

        _prozorroClient
            .GetTenderDetailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(CreateDetailResponse(cpvCode: "99999999-9"));

        await _sut.RunAsync();

        await _repository.DidNotReceive().UpsertAsync(
            Arg.Any<Tender>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RunAsync_NullDetailResponse_UpsertNotCalled()
    {
        _prozorroClient
            .GetTenderListAsync(Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns(CreateListResponse());

        _prozorroClient
            .GetTenderDetailAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((TenderDetailResponse?)null);

        await _sut.RunAsync();

        await _repository.DidNotReceive().UpsertAsync(
            Arg.Any<Tender>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RunAsync_EmptyList_UpsertNotCalled()
    {
        _prozorroClient
            .GetTenderListAsync(Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns(new TenderListResponse { Data = [], NextPage = null });

        await _sut.RunAsync();

        await _repository.DidNotReceive().UpsertAsync(
            Arg.Any<Tender>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RunAsync_MultipleTenders_UpsertsAllValid()
    {
        var listResponse = new TenderListResponse
        {
            Data =
            [
                new TenderListItem { Id = "tender1", DateModified = new DateTimeOffset(2025, 12, 15, 0, 0, 0, TimeSpan.Zero) },
                new TenderListItem { Id = "tender2", DateModified = new DateTimeOffset(2025, 12, 16, 0, 0, 0, TimeSpan.Zero) },
            ],
            NextPage = null
        };

        _prozorroClient
            .GetTenderListAsync(Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns(listResponse);

        _prozorroClient
            .GetTenderDetailAsync("tender1", Arg.Any<CancellationToken>())
            .Returns(CreateDetailResponse());

        _prozorroClient
            .GetTenderDetailAsync("tender2", Arg.Any<CancellationToken>())
            .Returns(CreateDetailResponse(status: "active")); 

        await _sut.RunAsync();

        await _repository.Received(1).UpsertAsync(
            Arg.Any<Tender>(),
            Arg.Any<CancellationToken>());
    }
}