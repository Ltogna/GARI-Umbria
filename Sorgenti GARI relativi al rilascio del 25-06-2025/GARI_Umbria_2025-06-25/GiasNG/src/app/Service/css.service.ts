/* eslint-disable */
import { DOCUMENT } from '@angular/common';
import { Inject, Injectable } from '@angular/core';
import { MasterService } from './master.service';

interface Css {
    name: string;
    src: string;
}

declare let document: any;

@Injectable({ providedIn: 'root' })
export class CssService {
    private css_s: any = {};

    private CssStore: Css[] = [];

    constructor(private masterService: MasterService,
        @Inject(DOCUMENT) private document: Document) {

        this.CssStore.push({
            name: 'kendoCommonBootstrap',
            src: this.masterService.link_GiasBase + '/kendoui/2021.1.119/styles/kendo.common-bootstrap.min.css'
        });

        this.CssStore.push({
            name: 'kendoBootstrap',
            src: this.masterService.link_GiasBase + '/kendoui/2021.1.119/styles/kendo.bootstrap.min.css'
        });

        this.CssStore.push({
            name: 'Gias_Kendo',
            src: this.masterService.link_GiasBase + '/kendoui/StylesGiasKendo/2021.1.119/Gias_Kendo.css'
        });

        this.CssStore.forEach((css: any) => {
            this.css_s[css.name] = {
                loaded: false,
                src: css.src
            };
        });
    }

    load(...css_s: string[]) {
        return new Promise(async (resolve, reject) => {
            const arrProm = new Array();
            if (css_s.length>0){
                css_s.forEach(
                    async (css) => {
                        arrProm.push(this.loadCss(css));
                    }
                );
                const resp = await Promise.all(arrProm);
            }
            resolve(true);
        });
    }

    loadCss(name: string) {
        return new Promise((resolve, reject) => {
            // resolve if already loaded
            if (this.css_s[name].loaded) {
                resolve({ css: name, loaded: true, status: 'Already Loaded' });
            } else {
                // load css
                const css = document.createElement('link');
                css.type = 'text/css';
                css.rel = 'stylesheet';
                css.href = this.css_s[name].src;
                css.id = name;
                if (css.readyState) {  // IE
                    css.onreadystatechange = () => {
                        if (css.readyState === 'loaded' || css.readyState === 'complete') {
                            css.onreadystatechange = null;
                            this.css_s[name].loaded = true;
                            resolve({ css: name, loaded: true, status: 'Loaded' });
                        }
                    };
                } else {  // Others
                    css.onload = () => {
                        this.css_s[name].loaded = true;
                        resolve({ css: name, loaded: true, status: 'Loaded' });
                    };
                }
                css.onerror = (error: any) => resolve({ css: name, loaded: false, status: 'Loaded' });
                document.getElementsByTagName('head')[0].appendChild(css);
            }
        });
    }


    loadStyle(styleName: string, id: string) {
        const head = this.document.getElementsByTagName('head')[0];

        const themeLink = this.document.getElementById(
            id
        ) as HTMLLinkElement;
        if (themeLink) {
            themeLink.href = styleName;
        } else {
            const style = this.document.createElement('link');
            style.id = id;
            style.rel = 'stylesheet';
            style.href = `${styleName}`;

            head.appendChild(style);
        }
    }

}
