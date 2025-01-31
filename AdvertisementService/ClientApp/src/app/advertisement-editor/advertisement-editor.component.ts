import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AdvertisementOutputModel, HttpService } from '../http.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-advertisement-editor',
  imports: [FormsModule],
  templateUrl: './advertisement-editor.component.html',
  styleUrl: './advertisement-editor.component.css'
})
export class AdvertisementEditorComponent {
  id:string="";
  imageSrc: string = "";
  selectedFile: File | null = null;
  isImageSelected: boolean = false;
  isImageAttached: boolean = false;
  text: string = "";

  @ViewChild('fileInput', { static: false }) fileInput: ElementRef | null = null;
  @ViewChild('telInput', { static: false }) telInput: ElementRef | null = null;

  constructor(private httpService: HttpService, private router: Router) {
    if (!this.httpService.isLogined || httpService.transferAdvertisementId == null)
      this.router.navigate(['log-in']);
    else
      httpService.getAdvertisement(httpService.transferAdvertisementId).subscribe({
    next: (data:AdvertisementOutputModel)=>{
      this.id = <string>httpService.transferAdvertisementId;
      httpService.transferAdvertisementId = null;
      if(data.imageLink!=null) {
        this.imageSrc = data.imageLink;
        this.isImageAttached = true;
        this.isImageSelected = true;
      }
      this.text = data.text;
      (this.telInput as ElementRef).nativeElement.value = data.number;
    }
  })
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
    this.httpService.editAdvertisement(this.id, number, this.text).subscribe({
      next: () => {
        if (this.isImageSelected && this.selectedFile!=null) {
          this.httpService.attachImage(this.id, this.selectedFile).subscribe({
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
        else if (!this.isImageSelected&&this.isImageAttached) {
          this.httpService.deleteImage(this.id).subscribe({
            next: () => {
              this.router.navigate(['profile']);
            },
            error: error => {              
              console.log(error);
              alert("Не удалось удалить изображение");              
              this.router.navigate(['profile']);
            }
          })
        }
        else {
          this.router.navigate(['profile']);
        }
      },
      error: error => {
        console.log(error);
        alert("Не удалось сохранить изменения");
      }
    })
  }
}
