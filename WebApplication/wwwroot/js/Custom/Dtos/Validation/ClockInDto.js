ClockInDto = (userId, timeShiftId, comments) => ({
    userId: userId,
    timeShiftId: timeShiftId,
    comments: comments,
    currentDate: new DateService().ConvertFrom.TimeStamp(Date.now()).To.Api()
});
