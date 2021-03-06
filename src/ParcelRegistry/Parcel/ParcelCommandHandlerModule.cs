namespace ParcelRegistry.Parcel
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Be.Vlaanderen.Basisregisters.CommandHandling;
    using Be.Vlaanderen.Basisregisters.CommandHandling.SqlStreamStore;
    using Be.Vlaanderen.Basisregisters.EventHandling;
    using Be.Vlaanderen.Basisregisters.GrAr.Provenance;
    using Commands.Crab;
    using Commands.Fixes;
    using SqlStreamStore;

    public sealed class ParcelCommandHandlerModule : CommandHandlerModule
    {
        public ParcelCommandHandlerModule(
            Func<IParcels> getParcels,
            Func<ConcurrentUnitOfWork> getUnitOfWork,
            Func<IStreamStore> getStreamStore,
            EventMapping eventMapping,
            EventSerializer eventSerializer,
            ParcelProvenanceFactory provenanceFactory,
            FixGrar1475ProvenanceFactory fixGrar1475ProvenanceFactory)
        {
            For<ImportTerrainObjectFromCrab>()
                .AddSqlStreamStore(getStreamStore, getUnitOfWork, eventMapping, eventSerializer)
                .AddProvenance(getUnitOfWork, provenanceFactory)
                .Handle(async (message, ct) => { await ImportTerrainObject(getParcels, message, ct); });

            For<ImportTerrainObjectHouseNumberFromCrab>()
                .AddSqlStreamStore(getStreamStore, getUnitOfWork, eventMapping, eventSerializer)
                .AddProvenance(getUnitOfWork, provenanceFactory)
                .Handle(async (message, ct) => { await ImportTerrainObjectHouseNumber(getParcels, message, ct); });

            For<ImportSubaddressFromCrab>()
                .AddSqlStreamStore(getStreamStore, getUnitOfWork, eventMapping, eventSerializer)
                .AddProvenance(getUnitOfWork, provenanceFactory)
                .Handle(async (message, ct) => { await ImportSubaddress(getParcels, message, ct); });

            For<FixGrar1475>()
                .AddSqlStreamStore(getStreamStore, getUnitOfWork, eventMapping, eventSerializer)
                .AddProvenance(getUnitOfWork, fixGrar1475ProvenanceFactory)
                .Handle(async (message, ct) => { await FixGrar1475(getParcels, message, ct); });
        }

        public async Task ImportSubaddress(
            Func<IParcels> getParcels,
            CommandMessage<ImportSubaddressFromCrab> message,
            CancellationToken ct)
        {
            var parcels = getParcels();

            var parcelId = ParcelId.CreateFor(message.Command.CaPaKey);
            var parcel = await parcels.GetAsync(parcelId, ct);

            parcel.ImportSubaddressFromCrab(
                message.Command.SubaddressId,
                message.Command.HouseNumberId,
                message.Command.BoxNumber,
                message.Command.BoxNumberType,
                message.Command.Lifetime,
                message.Command.Timestamp,
                message.Command.Operator,
                message.Command.Modification,
                message.Command.Organisation);
        }

        public async Task ImportTerrainObjectHouseNumber(
            Func<IParcels> getParcels,
            CommandMessage<ImportTerrainObjectHouseNumberFromCrab> message,
            CancellationToken ct)
        {
            var parcels = getParcels();

            var parcelId = ParcelId.CreateFor(message.Command.CaPaKey);
            var parcel = await parcels.GetAsync(parcelId, ct);

            parcel.ImportTerrainObjectHouseNumberFromCrab(
                message.Command.TerrainObjectHouseNumberId,
                message.Command.TerrainObjectId,
                message.Command.HouseNumberId,
                message.Command.Lifetime,
                message.Command.Timestamp,
                message.Command.Operator,
                message.Command.Modification,
                message.Command.Organisation);
        }

        public async Task ImportTerrainObject(
            Func<IParcels> getParcels,
            CommandMessage<ImportTerrainObjectFromCrab> message,
            CancellationToken ct)
        {
            var parcels = getParcels();

            var parcelId = ParcelId.CreateFor(message.Command.CaPaKey);
            var parcel = await parcels.GetOptionalAsync(parcelId, ct);

            if (!parcel.HasValue)
            {
                parcel = new Optional<Parcel>(Parcel.Register(parcelId, message.Command.CaPaKey));
                parcels.Add(parcelId, parcel.Value);
            }

            parcel.Value.ImportTerrainObjectFromCrab(
                message.Command.TerrainObjectId,
                message.Command.IdentifierTerrainObject,
                message.Command.TerrainObjectNatureCode,
                message.Command.XCoordinate,
                message.Command.YCoordinate,
                message.Command.BuildingNature,
                message.Command.Lifetime,
                message.Command.Timestamp,
                message.Command.Operator,
                message.Command.Modification,
                message.Command.Organisation);
        }

        public async Task FixGrar1475(
            Func<IParcels> getParcels,
            CommandMessage<FixGrar1475> message,
            CancellationToken ct)
        {
            var parcels = getParcels();
            var parcelId = message.Command.ParcelId;

            var parcel = await parcels.GetOptionalAsync(parcelId, ct);

            parcel.Value.FixGrar1475();
        }
    }
}
