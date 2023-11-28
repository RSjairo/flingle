import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'Flingle';
  users: any;

  constructor(private http: HttpClient) {}

  header = new HttpHeaders({
    'Content-Type': 'application/json',
    // Authorization: 'Access-Control-Allow-Origin', // Add any additional headers as needed
  });

  ngOnInit() {
    this.getUsers();
  }

  getUsers() {
    this.http
      .get(
        'http://localhost:5000/api/users'
        // { headers: this.header }
      )
      .subscribe(
        (response) => {
          this.users = response;
        },
        (error) => {
          console.log(error);
        }
      );
  }
}
