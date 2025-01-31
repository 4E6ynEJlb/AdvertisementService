import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpService } from '../http.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router'

@Component({
  selector: 'app-new-advertisement',
  imports: [FormsModule],
  templateUrl: './new-advertisement.component.html',
  styleUrl: './new-advertisement.component.css'
})
export class NewAdvertisementComponent {
  imageSrc: string = "";
  selectedFile: File | null = null;
  isImageSelected: boolean = false;
  text: string = "";

  @ViewChild('fileInput', { static: false }) fileInput: ElementRef | null = null;
  @ViewChild('telInput', { static: false }) telInput: ElementRef | null = null;

  constructor(private httpService: HttpService, private router: Router) {
    if (!this.httpService.isLogined)
    this.router.navigate(['log-in']);
   }

  imageSelectedEventHandler(event: Event) {
    this.selectedFile = <File>(event.target as HTMLInputElement).files?.item(0);
    let fileReader = new FileReader();
    fileReader.readAsDataURL(this.selectedFile as Blob);
    fileReader.addEventListener('load', (event) => {
      this.imageSrc = event.target?.result as string;
      this.isImageSelected = true;
    })
  }

  deleteButtonClick() {
    this.imageSrc = "";
    this.isImageSelected = false;
    (this.fileInput as ElementRef).nativeElement.value = null;
  }

  saveButtonClick() {
    if (!(this.telInput as ElementRef).nativeElement.checkValidity()) {
      alert("Некорректный номер телефона");
      return;
    }
    let number = <number>(this.telInput as ElementRef).nativeElement.value;
    this.httpService.createAdvertisement(number, this.text).subscribe({
      next: (data: string) => {
        if (this.isImageSelected) {
          this.httpService.attachImage(data, this.selectedFile!).subscribe({
            next: (data: string) => {
              this.router.navigate(['profile']);
            },
            error: error => {
              if ((<HttpErrorResponse>error).status != 200) {
                console.log(error);
                alert("Не удалось прикрепить изображение");
              }
              this.router.navigate(['profile']);
            }
          })
        }
        else {
          this.router.navigate(['profile']);
        }
      },
      error: error => {
        if ((<HttpErrorResponse>error).status == 403)
          alert("Достигнуто максимальное количество объявлений");
        else {
          console.log(error);
          alert("Не удалось сохранить объявление");
        }
      }
    })
  }
}
