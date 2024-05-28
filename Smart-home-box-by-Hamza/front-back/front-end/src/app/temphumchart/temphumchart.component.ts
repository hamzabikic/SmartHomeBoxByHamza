import {AfterViewInit, Component, OnInit, ViewChild, ElementRef, Renderer2, OnDestroy} from '@angular/core';
import {
  Chart, LineController, LineElement, PointElement, LinearScale, CategoryScale, Title, Tooltip, Legend
} from 'chart.js';
Chart.register(LineController, LineElement, PointElement, LinearScale, CategoryScale, Title, Tooltip, Legend);
import {initializeApp} from "firebase/app";
import {environment} from "../../environments/environment.prod";
import {TemperaturaVlaznost, temphumlista} from "../Klase/Klase";
import {HttpClient} from "@angular/common/http";
import {AuthService} from "../Services/AuthService";
import {child, get, getDatabase, ref} from "firebase/database";


@Component({
  selector: 'app-temphumchart',
  templateUrl: './temphumchart.component.html',
  styleUrls: ['./temphumchart.component.css']
})
export class TemphumchartComponent implements OnInit, OnDestroy{

  app = initializeApp(environment.firebaseConfig);
  temperatura ="";
  humidity ="";
  db:any;
  isHumidity = false;
  dropDown = "1";
  interval: NodeJS.Timeout | undefined = undefined;
  listaChart:any;
  temperaturechart:any;
  listener:any;
  constructor(private http: HttpClient, private auth: AuthService, private renderer: Renderer2) {
    this.db = getDatabase();

  }

  ngOnDestroy(): void {
        this.listener();
    }

  ucitajChart() {
    if(this.temperaturechart) {
      this.temperaturechart.destroy();
    }
    const ctx = document.getElementById('temperatureChart') as HTMLCanvasElement;
    let label ="";
    let data ="";
    let text ="";
    if(this.dropDown=="1") {
      label ="Time";
    }
    else {
      label = "Date"
    }
    if(this.isHumidity) {
         data="Humidity";
         text ="Humidity (%)";
    }
    else {
      data="Temperature";
      text ="Temperature (°C)";
    }

    this.temperaturechart= new Chart(ctx, {
      type: 'line',
      data: {
        labels: this.listaChart.labels,
        datasets: [{
          label: data,
          data: this.listaChart.data,
          borderColor: 'rgba(105, 159, 177, 1)',
          backgroundColor: 'rgba(105, 159, 177, 0.2)',
          fill: true
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
          x: {
            title: {
              display: true,
              text: label
            },
            ticks: {
              autoSkip: true,
              maxTicksLimit: 6,
              minRotation: 45,
              maxRotation: 45,
            }
          },
          y: {
            title: {
              display: true,
              text: text
            }
          }
        }
      }
    });
    }

  async ngOnInit() {
    this.ucitajPodatke();
    await this.ucitajChartPodatke();
    this.interval = setInterval(()=> {this.ucitajPodatke()},1000);
    this.listener = this.renderer.listen(window, 'resize',  () => {
      this.ucitajChart();
    });
  }
  async ucitajChartPodatke() {
    if(this.dropDown=="1") {
      this.listaChart = await this.http.get
      ("https://smarthomeapi.p2347.app.fit.ba/TemperaturaVlaznost/getTemperatureChartToday?isTemperature="+ !this.isHumidity).toPromise();
    }
    else if(this.dropDown=="7") {
      this.listaChart = await this.http.get
      ("https://smarthomeapi.p2347.app.fit.ba/TemperaturaVlaznost/getTemperatureChartLast7Days?isTemperature=" + !this.isHumidity).toPromise();
    }
    else if (this.dropDown =="30") {
      this.listaChart = await this.http.get
      ("https://smarthomeapi.p2347.app.fit.ba/TemperaturaVlaznost/getTemperatureChartLastMonth?isTemperature=" + !this.isHumidity).toPromise();
    }
    else {
      return;
    }
    this.ucitajChart();
  }

  ucitajPodatke () {
    get(child(ref(this.db), `${this.auth.getId()}/`)).then(
      (snapshot: any) => {
        if (snapshot.exists()) {
          this.temperatura = String(snapshot.val().Temperature);
          this.humidity = String(snapshot.val().Humidity);
        } else {
          console.log("Podaci nisu pronadjeni!");
        }
      }
    ).catch((err: any) => {
      console.log("Greska: " + err);
    });
  }
  tempcss() {
    if (parseInt(this.temperatura) < 5) {
      return {color: "blue", fontWeight: "bolder"};
    } else if (parseInt(this.temperatura) >= 5 && parseInt(this.temperatura) < 15) {
      return {color: "dodgerblue", fontWeight: "bolder"};
    } else if (parseInt(this.temperatura) >= 15 && parseInt(this.temperatura) < 25) {
      return {color: "green", fontWeight: "bolder"};
    } else if (parseInt(this.temperatura) >= 25 && parseInt(this.temperatura) < 35) {
      return {color: "orange", fontWeight: "bolder"};
    } else {
      return {color: "red", fontWeight: "bolder"};
    }
  }

}
