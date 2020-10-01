import { Constants } from "./constants";
import { DatePipe } from "@angular/common";

export module UtilsModule {
    export function buildListFromEnum<T>(enumData: T, addNullValue: boolean, nullIndicator: string = 'NullIndicatorLabel') {

        const keyValuePair = [];

        if (addNullValue) {
            keyValuePair.push({ key: nullIndicator, value: null});
        }

        for (const k of Object.keys(enumData)) {
            keyValuePair.push({ key: k, value: enumData[k] });
        }

        return keyValuePair;
    }

    export function encodeQueryData(data) {
        const ret = ['?'];

        for (const d in data) {
            if (d !== null && d !== undefined) {
                ret.push(encodeURIComponent(d) + '=' + encodeURIComponent(data[d]) + '&');
            }
        }

        const returnString = ret.join('');
        return returnString.substring(0, returnString.length - 1);
    }

    export function searchObjectsArray(nameKey, array) {
        for (let i = 0; i < array.length; i++) {
            if (array[i].key === nameKey) {
                return array[i];
            }
        }
    }

    export function addDays(date: Date, days: number) {
        date.setDate(date.getDate() + days);
        return date;
    }

    export function subDays(date: Date, days: number) {
        date.setDate(date.getDate() - days);
        return date;
    }

    export function formattedDate(date: Date) {
        let month = String(date.getMonth() + 1);
        let day = String(date.getDate());
        const year = String(date.getFullYear());

        if (month.length < 2) {
            month = '0' + month;
        }
        if (day.length < 2) {
            day = '0' + day;
        }

        return `${month}/${day}/${year}`;
    }

    export function formattedDateFromString(date: string, format?: string) {
        const datePipe: DatePipe = new DatePipe('en');
        const dateFormat = !format ? Constants.SYSTEM_DATE_TIME_FMT : format;
        return new Date(datePipe.transform(date, dateFormat));
    }

    export function dateInRangeByDaysSubtract(date: Date, dateRange: Date, diffDays: number) {

        const temp = dateRange;

        const beginRange = this.subDays(temp, diffDays);

        return beginRange <= date && date < dateRange;
    }

    export function compareDates(firstDate: Date, secondDate: Date) {
        if (firstDate === secondDate) {
            return 0;
        } else if (firstDate > secondDate) {
            return 1;
        } else if (firstDate < secondDate) {
            return -1;
        }
    }
}
