namespace ParcelRegistry.Projections.Syndication.Address
{
    using Be.Vlaanderen.Basisregisters.ProjectionHandling.Syndication;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddressOsloIdProjection : AtomEntryProjectionHandlerModule<AddressEvent, Address, SyndicationContext>
    {
        public AddressOsloIdProjection()
        {
            When(AddressEvent.AddressWasRegistered, AddSyndicationItemEntry);
            When(AddressEvent.AddressBecameComplete, AddSyndicationItemEntry);
            When(AddressEvent.AddressBecameIncomplete, AddSyndicationItemEntry);
            When(AddressEvent.AddressOsloIdWasAssigned, AddSyndicationItemEntry);
            When(AddressEvent.AddressWasRemoved, RemoveSyndicationItemEntry);
        }

        private static async Task AddSyndicationItemEntry(AtomEntry<Address> entry, SyndicationContext context, CancellationToken ct)
        {
            var latestItem = await context
                .AddressOsloIds
                .FindAsync(entry.Content.AddressId);

            if (latestItem == null)
            {
                latestItem = new AddressOlsoIdItem
                {
                    AddressId = entry.Content.AddressId,
                    Version = entry.Content.Identificator?.Versie.Value,
                    Position = long.Parse(entry.FeedEntry.Id),
                    OsloId = entry.Content.Identificator?.ObjectId,
                    IsComplete = entry.Content.IsComplete,
                };

                await context
                      .AddressOsloIds
                      .AddAsync(latestItem, ct);
            }
            else
            {
                latestItem.Version = entry.Content.Identificator?.Versie.Value;
                latestItem.Position = long.Parse(entry.FeedEntry.Id);
                latestItem.OsloId = entry.Content.Identificator?.ObjectId;
                latestItem.IsComplete = entry.Content.IsComplete;
            }
        }

        private static async Task RemoveSyndicationItemEntry(AtomEntry<Address> entry, SyndicationContext context, CancellationToken ct)
        {
            var latestItem = await context
                .AddressOsloIds
                .FindAsync(entry.Content.AddressId);

            latestItem.Version = entry.Content.Identificator?.Versie.Value;
            latestItem.Position = long.Parse(entry.FeedEntry.Id);
            latestItem.OsloId = entry.Content.Identificator?.ObjectId;
            latestItem.IsComplete = entry.Content.IsComplete;
            latestItem.IsRemoved = true;
        }
    }
}