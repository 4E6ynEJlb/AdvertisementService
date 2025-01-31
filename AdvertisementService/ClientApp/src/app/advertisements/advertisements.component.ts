import { Component, Directive, OnInit, ViewEncapsulation, Injector, ViewChild, ViewContainerRef, ComponentFactoryResolver, ComponentRef } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClient, HttpClientModule, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { AdvertisementComponent } from '../advertisement/advertisement.component';
import { GetAdvertisementsOptions, AdvertisementsPagesOutput, AdvertisementOutputModel, HttpService } from '../http.service';

@Component({
  selector: 'app-advertisements',
  imports: [
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule
  ],
  templateUrl: './advertisements.component.html',
  styleUrl: './advertisements.component.css',
  encapsulation: ViewEncapsulation.None
})

export class AdvertisementsComponent implements OnInit {
  ratingMin: number = 0.0
  ratingMax: number = 5.0
  ratingSliderFirst: number = 0.0
  ratingSliderSecond: number = 5.0
  keyWord: string = ""
  selectedValue: number = 0
  pagesCount: number = 0
  currentPage: number = 1
  prevHidden: boolean = true
  nextHidden: boolean = true
  useMinRating: boolean = false
  useMaxRating: boolean = false

  @ViewChild('container', { read: ViewContainerRef }) container!: ViewContainerRef;

  constructor(private httpService: HttpService) { }

  ngOnInit() {
    this.getData();
  }

  searchClick() {
    this.getData();
  }

  sliderValueChange() {
    if (this.ratingSliderFirst > this.ratingSliderSecond) {
      var temp = this.ratingSliderFirst;
      this.ratingSliderFirst = this.ratingSliderSecond;
      this.ratingSliderSecond = temp;
    }
    this.ratingMin = this.ratingSliderFirst;
    this.ratingMax = this.ratingSliderSecond;
  }

  valueChange() {
    if (this.ratingMin > this.ratingMax) {
      var temp = this.ratingMin;
      this.ratingMin = this.ratingMax;
      this.ratingMax = temp;
    }
    this.ratingSliderFirst = this.ratingMin;
    this.ratingSliderSecond = this.ratingMax;
  }

  getData() {
    var options = new GetAdvertisementsOptions();
    options.criterion = this.selectedValue > 1 ? 0 : 1;
    options.isASC = this.selectedValue % 2 == 1;
    options.keyWord = this.keyWord == "" ? null : this.keyWord;
    options.page = this.currentPage;
    options.pageSize = 5;
    options.ratingLow = this.useMinRating ? this.ratingMin : undefined;
    options.ratingHigh = this.useMaxRating ? this.ratingMax : undefined;

    this.httpService.getAdvertisements(options).subscribe({
      next: (data: AdvertisementsPagesOutput) => {
        this.parseData(data);
      },
      error: error => {
        console.log(error);
        if ((<HttpErrorResponse>error).status == 400 && this.currentPage > 1)
          this.currentPage -= 1;
      }
    });
  }

  parseData(data: AdvertisementsPagesOutput) {
    this.container.clear();
    this.pagesCount = data.pagesCount;
    var ads = data.advertisements;
    ads?.forEach(element => {
      var injector = Injector.create({
        providers: [
          { provide: 'ADVERTISEMENT', useValue: element },
        ],
      });
      this.container.createComponent(AdvertisementComponent, { injector: injector });

    });
    if (this.currentPage == 1)
      this.prevHidden = true;
    else
      this.prevHidden = false;
    if (this.currentPage >= this.pagesCount)
      this.nextHidden = true;
    else
      this.nextHidden = false;
  }
  
  changePage(direction: boolean, toEnd: boolean) {
    var switchArg = (direction ? 2 : 0) + (toEnd ? 1 : 0);
    var pageNumber = 0;
    switch (switchArg) {
      case 0: {
        pageNumber = this.currentPage - 1;
        break;
      }
      case 1: {
        pageNumber = 1;
        break;
      }
      case 2: {
        pageNumber = this.currentPage + 1;
        break;
      }
      case 3: {
        pageNumber = this.pagesCount;
        break;
      }
    }
    this.currentPage = pageNumber;
    this.getData();
  }
}