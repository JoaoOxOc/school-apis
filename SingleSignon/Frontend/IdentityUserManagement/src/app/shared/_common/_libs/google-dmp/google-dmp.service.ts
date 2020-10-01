import { Injectable, OnInit } from '@angular/core';
import { DiffMatchPatch, DiffOp } from './google-dmp';

@Injectable()
export class GoogleDMPService implements OnInit {

    constructor(private dmp: DiffMatchPatch) { }

    ngOnInit() {

    }

    getDiff(left: string, right: string) {
        return this.dmp.diff_main(left, right);
    }

    getSemanticDiff(left: string, right: string) {
        const diffs = this.dmp.diff_main(left, right);
        this.dmp.diff_cleanupSemantic(diffs);
        return diffs;
    }

    getProcessingDiff(left: string, right: string) {
        const diffs = this.dmp.diff_main(left, right);
        this.dmp.diff_cleanupEfficiency(diffs);
        return diffs;
    }

    getLineDiff(left: string, right: string) {
        const chars = this.dmp.diff_linesToChars_(left, right);
        const diffs = this.dmp.diff_main(chars.chars1, chars.chars2, false);
        this.dmp.diff_charsToLines_(diffs, chars.lineArray);
        return diffs;
    }

    getDmp() {
        return this.dmp;
    }

    getHtml(diffs) {
        let html;
        html = '<div class="inline">';

        for (let _i = 0, diffs_1 = diffs; _i < diffs_1.length; _i++) {
            const diff = diffs_1[_i];
            diff[1] = diff[1].replace(/\n/g, '<br/>');
            if (diff[0] === 0 /* Equal */) {
                html += '<span class="equal">' + diff[1] + '</span>';
            }
            if (diff[0] === -1 /* Delete */) {
                html += '<del>' + diff[1] + '</del>';
            }
            if (diff[0] === 1 /* Insert */) {
                html += '<ins>' + diff[1] + '</ins>';
            }
        }
        html += '</div>';
        return html;
    }
}
