HasOverlapDto = (startOnTs, endOnTs, excludeStartOnTs, excludeEndOnTs, isEdit, employeeIds) => ({
    startOn: new DateService().ConvertFrom.TimeStamp(startOnTs).To.Api(),
    endOn: new DateService().ConvertFrom.TimeStamp(endOnTs).To.Api(),
    excludeStartOn: new DateService().ConvertFrom.TimeStamp(excludeStartOnTs).To.Api(),
    excludeEndOn: new DateService().ConvertFrom.TimeStamp(excludeEndOnTs).To.Api(),
    isEdit: isEdit,
    employeeIds: employeeIds
});
