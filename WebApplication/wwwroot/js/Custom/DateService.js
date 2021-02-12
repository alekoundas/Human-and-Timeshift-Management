class DateService {

    constructor() {
        this._startOnTs = 0;
        this._endOnTs = 0;
        this._dateTs = 0;
    }

    #Convert = () => ({
        To: ({
            Api: () => { return this.#CompleteTimeWithZerosForApiPost(this._dateTs) },
            YYYMMDDHHMM: () => { return "poytses" }
        })
    })

    ConvertFrom = ({
        Api: (date) => {
            this._dateTs = moment(date, 'YYYY/MM/DD HH:mm:ss').valueOf();
            return this.#Convert();
        },
        TimeStamp: (dateTs) => {
            this._dateTs = dateTs;
            return this.#Convert();
        }
    })


    #CompleteTimeWithZerosForApiPost = dateTs => {
        if (dateTs) {

            var date = new Date(+dateTs);
            var year = date.getFullYear();
            var month = String((date.getMonth() + 1)).length == 1 ?
                '0' + (date.getMonth() + 1) : (date.getMonth() + 1);
            var day = String(date.getDate()).length == 1 ?
                '0' + date.getDate() : date.getDate();
            var hour = String(date.getHours()).length == 1 ?
                '0' + date.getHours() : date.getHours();
            var min = String(date.getMinutes()).length == 1 ?
                '0' + date.getMinutes() : date.getMinutes();

            return year + '-' + month + '-' + day + 'T' + hour + ':' + min;
        }
        else
            return undefined;
    }
};
