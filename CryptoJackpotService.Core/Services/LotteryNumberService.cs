using AutoMapper;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Database.Models;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.Constants;
using CryptoJackpotService.Models.DTO.LotteryNumber;
using CryptoJackpotService.Models.Enums;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Models.Responses;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Core.Services;

public class LotteryNumberService(
    IMapper mapper,
    ILotteryNumberRepository lotteryNumberRepository,
    ILotteryRepository lotteryRepository,
    IStringLocalizer<ISharedResource> localizer) : BaseService(mapper), ILotteryNumberService
{
    /// <summary>
    /// Obtiene números aleatorios disponibles para compra
    /// </summary>
    public async Task<ResultResponse<List<LotteryNumberDto>>> GetAvailableNumbersAsync(Guid lotteryId, int count = 10)
    {
        var lottery = await lotteryRepository.GetLotteryAsync(lotteryId);
        if (lottery is null)
            return ResultResponse<List<LotteryNumberDto>>.Failure(ErrorType.NotFound, localizer[ValidationMessages.LotteryNotFound]);

        var maxNumber = lottery.MaxNumber - lottery.MinNumber + 1;
        var availableNumbers = await lotteryNumberRepository.GetRandomAvailableNumbersAsync(
            lotteryId, count, maxNumber, lottery.MinNumber);

        var result = availableNumbers.Select(n => new LotteryNumberDto
        {
            Number = n,
            LotteryId = lotteryId,
            IsAvailable = true
        }).ToList();

        return ResultResponse<List<LotteryNumberDto>>.Ok(result);
    }

    /// <summary>
    /// Verifica si un número específico está disponible
    /// </summary>
    public async Task<ResultResponse<bool>> IsNumberAvailableAsync(Guid lotteryId, int number, int series)
    {
        var isAvailable = await lotteryNumberRepository.IsNumberAvailableAsync(lotteryId, number, series);
        return ResultResponse<bool>.Ok(isAvailable);
    }

    /// <summary>
    /// Reserva números para un ticket (los marca como no disponibles)
    /// </summary>
    public async Task<ResultResponse<List<LotteryNumberDto>>> ReserveNumbersAsync(
        Guid lotteryId, Guid ticketId, List<int> numbers, int series)
    {
        var lottery = await lotteryRepository.GetLotteryAsync(lotteryId);
        if (lottery is null)
            return ResultResponse<List<LotteryNumberDto>>.Failure(ErrorType.NotFound, localizer[ValidationMessages.LotteryNotFound]);

        // Validar que los números estén en el rango permitido
        var invalidNumbers = numbers.Where(n => n < lottery.MinNumber || n > lottery.MaxNumber).ToList();
        if (invalidNumbers.Any())
            return ResultResponse<List<LotteryNumberDto>>.Failure(ErrorType.BadRequest,
                $"Números fuera de rango: {string.Join(", ", invalidNumbers)}");

        // Validar que la serie sea válida
        if (series < 1 || series > lottery.TotalSeries)
            return ResultResponse<List<LotteryNumberDto>>.Failure(ErrorType.BadRequest,
                $"Serie inválida. Debe estar entre 1 y {lottery.TotalSeries}");

        // Verificar disponibilidad de todos los números
        foreach (var number in numbers)
        {
            var isAvailable = await lotteryNumberRepository.IsNumberAvailableAsync(lotteryId, number, series);
            if (!isAvailable)
                return ResultResponse<List<LotteryNumberDto>>.Failure(ErrorType.Conflict,
                    $"El número {number} serie {series} ya no está disponible");
        }

        // Crear los registros de números reservados
        var lotteryNumbers = numbers.Select(n => new LotteryNumber
        {
            Id = Guid.NewGuid(),
            LotteryId = lotteryId,
            Number = n,
            Series = series,
            IsAvailable = false,
            TicketId = ticketId
        }).ToList();

        await lotteryNumberRepository.AddRangeAsync(lotteryNumbers);

        var result = Mapper.Map<List<LotteryNumberDto>>(lotteryNumbers);
        return ResultResponse<List<LotteryNumberDto>>.Created(result);
    }

    /// <summary>
    /// Libera números reservados (cuando se cancela una compra)
    /// </summary>
    public async Task<ResultResponse<bool>> ReleaseNumbersAsync(Guid ticketId)
    {
        var released = await lotteryNumberRepository.ReleaseNumbersByTicketAsync(ticketId);
        return released
            ? ResultResponse<bool>.Ok(true)
            : ResultResponse<bool>.Failure(ErrorType.NotFound, "No se encontraron números para liberar");
    }

    /// <summary>
    /// Obtiene estadísticas de números vendidos por lotería
    /// </summary>
    public async Task<ResultResponse<LotteryNumberStatsDto>> GetNumberStatsAsync(Guid lotteryId)
    {
        var lottery = await lotteryRepository.GetLotteryAsync(lotteryId);
        if (lottery is null)
            return ResultResponse<LotteryNumberStatsDto>.Failure(ErrorType.NotFound, localizer[ValidationMessages.LotteryNotFound]);

        var soldNumbers = await lotteryNumberRepository.GetSoldNumbersAsync(lotteryId);
        var totalPossible = (lottery.MaxNumber - lottery.MinNumber + 1) * lottery.TotalSeries;

        var stats = new LotteryNumberStatsDto
        {
            LotteryId = lotteryId,
            TotalNumbers = totalPossible,
            SoldNumbers = soldNumbers.Count,
            AvailableNumbers = totalPossible - soldNumbers.Count,
            PercentageSold = totalPossible > 0 
                ? Math.Round((decimal)soldNumbers.Count / totalPossible * 100, 2) 
                : 0
        };

        return ResultResponse<LotteryNumberStatsDto>.Ok(stats);
    }
}