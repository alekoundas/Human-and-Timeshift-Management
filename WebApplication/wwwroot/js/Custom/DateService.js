class DateService {

    constructor() {
        this._startOnTs = 0;
        this._endOnTs = 0;
        this._dateTs = 0;
    }

    ConvertFrom = ({
        Api: (date) => {
            if (date)
                this._dateTs = moment(date, 'YYYY/MM/DD HH:mm:ss').valueOf();
            return this.#Convert();
        },
        Date: (date) => {
            if (date)
                this._dateTs = date.valueOf();
            return this.#Convert();
        },
        Input: (date) => {
            if (date)
                this._dateTs = moment(date, 'YYYY/MM/DD HH:mm:ss').valueOf();
            return this.#Convert();
        },
        TimeStamp: (date) => {
            if (date)
                this._dateTs = date;
            return this.#Convert();
        },
        String: ({//ex. 18/03/2021 22:19
            DDMMYYYY_HHMM: (date) => {
                this._dateTs = moment(date, 'DD/MM/YYYY HH:mm:ss').valueOf();
                return this.#Convert();
            }
        })
    })

    GetDaysOfMonth = (year, month) =>
        new Date(year, month, 0).getDate();

    #Convert = () => ({
        To: ({
            Api: () => this.#CompleteTimeWithZerosForApiPost(this._dateTs),
            Date: () => new Date(this._dateTs),
            TimeStamp: () => this._dateTs,
            Input: () => this.#CompleteTimeWithZerosForApiPost(this._dateTs),
            DDMMYYYYHHMM: () => moment(new Date(this._dateTs)).format('DD/MM/YYYY HH:MM')
        })
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
