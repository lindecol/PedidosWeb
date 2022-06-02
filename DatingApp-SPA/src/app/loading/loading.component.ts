
import { LoadingService } from '../_services/loading.service';
import { Subscription } from 'rxjs/internal/Subscription';
import { debounceTime } from "rxjs/operators";

import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.css']
})
export class LoadingComponent implements AfterViewInit,OnDestroy {

  debounceTime: number = 200;
  loading: boolean = false;
  loadingSubscription: Subscription;

  constructor(private loadingScreenService: LoadingService, 
    private _elmRef: ElementRef,
    private _changeDetectorRef: ChangeDetectorRef) {
  }

  ngAfterViewInit(): void {
    this._elmRef.nativeElement.style.display = 'none';
    this.loadingSubscription = this.loadingScreenService.loadingStatus.pipe(debounceTime(this.debounceTime)).subscribe(
      (status: boolean) => {
        this._elmRef.nativeElement.style.display = status ? 'block' : 'none';       
        this._changeDetectorRef.detectChanges();
      }
    );
  }

  ngOnDestroy() {
    this.loadingSubscription.unsubscribe();
  }
}
