Select2Dto = (search, page, fromEntityId, existingIds) => ({
    search: search,
    page: page || 1,
    fromEntityId: fromEntityId != undefined ?
        fromEntityId.length == 36 ? undefined : fromEntityId
        :
        undefined,
    fromEntityIdString: fromEntityId != undefined ?
        fromEntityId.length != 36 ? undefined : fromEntityId
        :
        undefined,
    existingIds:
        existingIds != undefined ?
            [...existingIds].filter(x => x.length == 36).length > 0 ? undefined : existingIds
            :
            undefined,
    existingIdsString:
        existingIds != undefined ?
            [...existingIds].filter(x => x.length != 36).length > 0 ? undefined : existingIds
            :
            undefined
});

