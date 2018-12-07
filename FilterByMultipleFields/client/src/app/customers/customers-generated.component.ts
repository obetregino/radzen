/*
  This file is automatically generated. Any changes will be overwritten.
  Modify customers.component.ts instead.
*/
import { ChangeDetectorRef, ViewChild, AfterViewInit, Injector, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Subscription } from 'rxjs/Subscription';

import { DialogService, DIALOG_PARAMETERS, DialogRef } from '@radzen/angular/dist/dialog';
import { NotificationService } from '@radzen/angular/dist/notification';
import { ContentComponent } from '@radzen/angular/dist/content';
import { HeadingComponent } from '@radzen/angular/dist/heading';
import { LabelComponent } from '@radzen/angular/dist/label';
import { TextBoxComponent } from '@radzen/angular/dist/textbox';
import { DropDownComponent } from '@radzen/angular/dist/dropdown';
import { ButtonComponent } from '@radzen/angular/dist/button';
import { FieldsetComponent } from '@radzen/angular/dist/fieldset';
import { GridComponent } from '@radzen/angular/dist/grid';

import { NorthwindService } from '../northwind.service';

export class CustomersGenerated implements AfterViewInit, OnInit, OnDestroy {
  // Components
  @ViewChild('content1') content1: ContentComponent;
  @ViewChild('pageTitle') pageTitle: HeadingComponent;
  @ViewChild('label0') label0: LabelComponent;
  @ViewChild('textbox0') textbox0: TextBoxComponent;
  @ViewChild('label1') label1: LabelComponent;
  @ViewChild('textbox1') textbox1: TextBoxComponent;
  @ViewChild('dropdown0') dropdown0: DropDownComponent;
  @ViewChild('button0') button0: ButtonComponent;
  @ViewChild('fieldset0') fieldset0: FieldsetComponent;
  @ViewChild('customersGrid') customersGrid: GridComponent;

  router: Router;

  cd: ChangeDetectorRef;

  route: ActivatedRoute;

  notificationService: NotificationService;

  dialogService: DialogService;

  dialogRef: DialogRef;

  _location: Location;

  _subscription: Subscription;

  northwind: NorthwindService;

  getCustomersResult: any;

  getCustomersCount: any;

  operators: any;

  operator: any;

  parameters: any;

  City: any;

  Country: any;

  constructor(private injector: Injector) {
  }

  ngOnInit() {
    this.notificationService = this.injector.get(NotificationService);

    this.dialogService = this.injector.get(DialogService);

    this.dialogRef = this.injector.get(DialogRef, null);

    this.router = this.injector.get(Router);

    this.cd = this.injector.get(ChangeDetectorRef);

    this._location = this.injector.get(Location);

    this.route = this.injector.get(ActivatedRoute);

    this.northwind = this.injector.get(NorthwindService);
  }

  ngAfterViewInit() {
    this._subscription = this.route.params.subscribe(parameters => {
      if (this.dialogRef) {
        this.parameters = this.injector.get(DIALOG_PARAMETERS);
      } else {
        this.parameters = parameters;
      }
      this.load();
      this.cd.detectChanges();
    });
  }

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }


  load() {
    this.northwind.getCustomers(null, this.customersGrid.allowPaging ? this.customersGrid.pageSize : null, this.customersGrid.allowPaging ? 0 : null, null, null, this.customersGrid.allowPaging)
    .subscribe((result: any) => {
      this.getCustomersResult = result.value;

      this.getCustomersCount = this.customersGrid.allowPaging ? result['@odata.count'] : result.value.length;
    }, (result: any) => {

    });

    this.operators = [{name: 'and', value: 'and'}, {name: 'or', value: 'or'}];

    this.operator = 'and';
  }

  textbox0Change(event: any) {
    this.City = event;
  }

  textbox1Change(event: any) {
    this.Country = event;
  }

  dropdown0Change(event: any) {
    this.customersGrid.callLoadData();
  }

  button0Click(event: any) {
    this.customersGrid.skip = 0;

    this.customersGrid.callLoadData();
  }

  customersGridLoadData(event: any) {
    this.northwind.getCustomers(`${(<any>this).filter(event.filter)}`, event.top, event.skip, `${event.orderby}`, ``, event.top != null && event.skip != null)
    .subscribe((result: any) => {
      this.getCustomersResult = result.value;

      this.getCustomersCount = event.top != null && event.skip != null ? result['@odata.count'] : result.value.length;
    }, (result: any) => {

    });
  }
}
