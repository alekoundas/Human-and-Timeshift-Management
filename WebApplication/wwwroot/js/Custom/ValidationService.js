
class ValidationService {
    constructor() {
    }

    #AjaxPost = async (url, data) =>
        await $.ajax({
            type: "POST",
            url: "/api/validations/" + url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data)
        });


    WorkHours = () => ({
        HasOverlapRange: (dtos) => this.#AjaxPost('HasOverlapRangeWorkHours', dtos),
        HasOverlap: (dto) => this.#AjaxPost('HasOverlapRangeWorkHours', [...dto]),
        HasOverTimeRange: (dtos) => this.#AjaxPost('HasOverTimeRangeWorkHours', dtos),
        HasOverTime: (dto) => this.#AjaxPost('HasOverTimeRangeWorkHours', [...dto])
    })
    RealWorkHours = () => ({
        HasOverlapRange: (dtos) => this.#AjaxPost('HasOverlapRangeRealWorkHours', dtos),
        HasOverlap: (dto) => this.#AjaxPost('HasOverlapRangeRealWorkHours', [...dto]),
        HasOverTimeRange: (dtos) => this.#AjaxPost('HasOverTimeRangeRealWorkHours', dtos),
        HasOverTime: (dto) => this.#AjaxPost('HasOverTimeRangeRealWorkHours', [...dto])
    })


    //HasOverlapRangeWorkHours = (hasOverlapDtos) => {
    //    return this.#AjaxPost('HasOverlapRangeWorkHours', hasOverlapDtos)
    //}
    //HasOverlapWorkHours = (hasOverlapDto) => {
    //    return this.#AjaxPost('HasOverlapRangeWorkHours', [...hasOverlapDto])
    //}
};
