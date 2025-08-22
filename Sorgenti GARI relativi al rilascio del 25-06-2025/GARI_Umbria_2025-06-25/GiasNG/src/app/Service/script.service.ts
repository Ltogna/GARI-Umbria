/* eslint-disable */
import { Injectable } from '@angular/core';
import { MasterService } from './master.service';
import { get } from 'scriptjs';

interface Scripts {
    name: string;
    src: string;
}

declare let document: any;

@Injectable({ providedIn: 'root' })
export class ScriptService {
    private scripts: any = {};

    private ScriptStore: Scripts[] = [ ];

    constructor(private masterService: MasterService) {

        this.ScriptStore.push({
            name: 'jszip',
            src: this.masterService.link_GiasBase + '/kendoui/2021.1.119/js/jszip.min.js'
        });

        this.ScriptStore.push({
            name: 'kendo_all',
            src: this.masterService.link_GiasBase + '/kendoui/2021.1.119/js/kendo.all.min.js'
        });

        this.ScriptStore.push({
            name: 'kendo_messages',
            src: this.masterService.link_GiasBase + '/kendoui/2021.1.119/js/messages/kendo.messages.it-IT.min.js'
        });

        this.ScriptStore.push({
            name: 'kendo_culture',
            src: this.masterService.link_GiasBase + '/kendoui/2021.1.119/js/cultures/kendo.culture.it-IT.min.js'
        });

        this.ScriptStore.push({
            name: 'FunzioniComuni_kendoGrid',
            src: this.masterService.link_GiasBase + '/kendoui/ScriptsGiasKendo/2021.1.119/funzioniComuniKendoGrid.js'
        });

        this.ScriptStore.push({
            name: 'jQuery',
            src: 'https://code.jquery.com/jquery-3.3.1.js'
        });

        this.ScriptStore.forEach((script: any) => {
            this.scripts[script.name] = {
                loaded: false,
                src: script.src
            };
        });
    }

    load(...scripts: string[]) {
        return new Promise(async (resolve, reject) => {
            for (let i = 0; i < scripts.length; i++) {
                const resp = await this.loadScript(scripts[i]);
            }
            resolve(true);
        });
    }

    loadScript(name: string) {
        return new Promise((resolve, reject) => {
            // resolve if already loaded
            if (this.scripts[name].loaded) {
                resolve({ script: name, loaded: true, status: 'Already Loaded' });
            } else {
                // load script
                const script = document.createElement('script');
                script.type = 'text/javascript';
                script.src = this.scripts[name].src;
                if (script.readyState) {  // IE
                    script.onreadystatechange = () => {
                        if (script.readyState === 'loaded' || script.readyState === 'complete') {
                            script.onreadystatechange = null;
                            this.scripts[name].loaded = true;
                            resolve({ script: name, loaded: true, status: 'Loaded' });
                        }
                    };
                } else {  // Others
                    script.onload = () => {
                        this.scripts[name].loaded = true;
                        resolve({ script: name, loaded: true, status: 'Loaded' });
                    };
                }
                script.onerror = (error: any) => resolve({ script: name, loaded: false, status: 'Loaded' });
                document.getElementsByTagName('head')[0].appendChild(script);
            }
        });
    }

}
