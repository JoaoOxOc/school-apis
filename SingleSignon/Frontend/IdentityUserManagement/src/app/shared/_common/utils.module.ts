import { Constants } from './constants';
import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable()
export class UtilsModule {

    constructor(private translate: TranslateService) {
        // super('pt');
    }

    selectListFromEnum<T>(enumData: T, addNullValue: boolean, nullIndicator: string = 'NullIndicatorLabel') {

        const keyValuePair = [];

        if (addNullValue) {
            keyValuePair.push({ key: nullIndicator, value: null });
        }

        for (const k of Object.keys(enumData)) {
            keyValuePair.push({ key: k, value: enumData[k] });
        }

        return keyValuePair;
    }

    buildListFromEnum<T>(enumData: T, addNullValue: boolean, ignoreNumbers = true, nullIndicator: string = 'NullIndicatorLabel') {

        const keyValuePair = [];

        if (addNullValue) {
            keyValuePair.push({ key: nullIndicator, value: null });
        }

        for (const k of Object.keys(enumData).filter(k => typeof enumData[k as any] === 'number')) {
            keyValuePair.push({ key: k, value: enumData[k] });
        }

        return keyValuePair;
    }

    compareDates(firstDate: Date, secondDate: Date) {
        if (firstDate === secondDate) {
            return 0;
        } else if (firstDate > secondDate) {
            return 1;
        } else if (firstDate < secondDate) {
            return -1;
        }
    }

    compareDatesWithoutTime(firstDate: Date, secondDate: Date) {
        const fDate = new Date();
        const sDate = new Date();
        fDate.setTime(firstDate.getTime());
        sDate.setTime(secondDate.getTime());
        fDate.setHours(3,0,0,0);
        sDate.setHours(3,0,0,0);
        if (fDate == secondDate) {
            return 0;
        } else if (fDate > sDate) {
            return 1;
        } else if (fDate < sDate) {
            return -1;
        } else {
            return 0;
        }
    }

    private adjustDate(date: Date, days) {
        let adjustedDate;
        days = days || 0;

        if(days === 0){
            adjustedDate = new Date( date.getTime() );
        } else if(days > 0) {
            adjustedDate = new Date( date.getTime() );

            adjustedDate.setDate(date.getDate() + days);
        } else {
            adjustedDate = new Date(
                date.getFullYear(),
                date.getMonth(),
                date.getDate() - Math.abs(days),
                date.getHours(),
                date.getMinutes(),
                date.getSeconds(),
                date.getMilliseconds()
            );
        }

        return adjustedDate;
    };
    
    addDays(date: Date, days: number) {
        return this.adjustDate(date, days);
    }

    subDays(date: Date, days: number) {
        return this.adjustDate(date, days * -1);
    }

    formattedDate(date: Date) {
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

    formattedDateFromString(date: string, format?: string) {
        const datePipe: DatePipe = new DatePipe('en');
        date = date.replace(/-/g, '/');
        const dateTimeParts = date.split(' ');
        const dateParts = dateTimeParts[0].split('/');
        const timeParts = dateTimeParts[1].split(':');
        format = format ? format.replace(/-/g, '/') : format;
        const dateFormat = !format ? Constants.SYSTEM_DATE_TIME_FMT : format;
        // const d = datePipe.transform(date, dateFormat);
        return new Date(Number(dateParts[2]),Number(dateParts[1]) - 1,Number(dateParts[0]),Number(timeParts[0]),Number(timeParts[1]),Number(timeParts[2]));
    }

    dateInRangeByDaysSubtract(date: Date, dateRange: Date, diffDays: number) {

        const beginRange = this.subDays(dateRange, diffDays);
    
        return beginRange <= date && date < dateRange;
    }

    dateInRangeByDaysAdd(date: Date, dateRange: Date, diffDays: number) {

        const endRange = this.addDays(dateRange, diffDays);
    
        return dateRange <= date && date < endRange;
    }
}
