namespace ProzorroDataMining.Application.Mappers;

using ProzorroDataMining.Contracts.DTOs.Prozorro;
using ProzorroDataMining.Domain.Entities;

public static class TenderMapper
{
    public static Tender? MapToEntity(TenderData data)
    {
        if (string.IsNullOrEmpty(data.Id)) return null;
        if (data.Value is null) return null;
        if (data.ProcuringEntity is null) return null;

        var tender = Tender.Create(
            id: data.Id,
            status: data.Status,
            budgetAmount: data.Value.Amount,
            budgetCurrency: data.Value.Currency,
            procuringEntityId: data.ProcuringEntity.Identifier?.Id,
            procuringEntityName: data.ProcuringEntity.Name,
            dateCreated: data.DateCreated,
            dateModified: data.DateModified);

        foreach (var item in data.Items ?? [])
        {
            if (item.Classification is null) continue;

            tender.AddItem(TenderItem.Create(
                id: item.Id,
                tenderId: data.Id,
                cpvCode: item.Classification.Id,
                cpvDescription: item.Classification.Description,
                description: item.Description,
                quantity: item.Quantity,
                unitName: item.Unit?.Name,
                unitCode: item.Unit?.Code));
        }

        foreach (var contract in data.Contracts ?? [])
        {
            tender.AddContract(TenderContract.Create(
                id: contract.Id,
                tenderId: data.Id,
                contractValue: contract.Value?.Amount,
                currency: contract.Value?.Currency ?? "UAH",
                status: contract.Status,
                dateSigned: contract.DateSigned));
        }

        foreach (var award in data.Awards ?? [])
        {
            foreach (var supplier in award.Suppliers ?? [])
            {
                tender.AddAward(TenderAward.Create(
                    id: award.Id,
                    tenderId: data.Id,
                    awardValue: award.Value?.Amount,
                    status: award.Status,
                    supplierName: supplier.Name,
                    supplierId: supplier.Identifier?.Id));
            }
        }

        return tender;
    }
}