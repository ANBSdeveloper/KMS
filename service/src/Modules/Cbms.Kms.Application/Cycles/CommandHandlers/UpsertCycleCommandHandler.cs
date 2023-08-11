using Cbms.Domain.Entities;
using Cbms.Domain.Repositories;
using Cbms.Extensions;
using Cbms.Kms.Application.Cycles.Commands;
using Cbms.Kms.Application.Cycles.Dto;
using Cbms.Kms.Application.Cycles.Query;
using Cbms.Kms.Domain;
using Cbms.Kms.Domain.Cycles;
using Cbms.Kms.Domain.Cycles.Actions;
using Cbms.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace Cbms.Kms.Application.Cycles.CommandHandlers
{
    public class UpsertCycleCommandHandler : UpsertEntityCommandHandler<UpsertCycleCommand, GetCycle, CycleDto>
    {
        private readonly IRepository<Cycle, int> _cycleRepository;

        public UpsertCycleCommandHandler(IRequestSupplement supplement, IRepository<Cycle, int> CycleRepository) : base(supplement)
        {
            LocalizationSourceName = KmsConsts.LocalizationSourceName;
            _cycleRepository = CycleRepository;
        }

        protected override async Task<CycleDto> HandleCommand(UpsertCycleCommand request, CancellationToken cancellationToken)
        {
            var entityDto = request.Data;

            Cycle entity = null;
            if (!request.Data.Id.IsNew())
            {
                entity = await _cycleRepository.GetAsync(request.Data.Id);
            }

            if (entity == null)
            {
                entity = Cycle.Create();
                await _cycleRepository.InsertAsync(entity);
            }

            entityDto.FromDate = entityDto.FromDate.ToLocalTime().BeginOfDay();
            entityDto.ToDate = entityDto.ToDate.ToLocalTime().EndOfDay();

            var cycle = await _cycleRepository.FirstOrDefaultAsync(p => p.Id != entity.Id && ((p.FromDate >= entityDto.FromDate && p.FromDate <= entityDto.ToDate)
            || (p.ToDate >= entityDto.FromDate && p.ToDate <= entityDto.ToDate)
            || (p.FromDate <= entityDto.FromDate && p.ToDate >= entityDto.FromDate)
            || (p.FromDate <= entityDto.ToDate && p.ToDate >= entityDto.ToDate)));

            if (cycle != null)
            {
                throw BusinessExceptionBuilder.Create(LocalizationSource).MessageCode("Cycle.Error_FromDate_ToDate", cycle.Number).Build();
            }

            

            await entity.ApplyActionAsync(new UpsertCycleAction(
                entityDto.Year,
                entityDto.Number,
                entityDto.FromDate,
                entityDto.ToDate,
                entityDto.IsActive
            ));

            await _cycleRepository.UnitOfWork.CommitAsync(cancellationToken);

            return await GetEntityDtoAsync(entity.Id);
        }
    }
}