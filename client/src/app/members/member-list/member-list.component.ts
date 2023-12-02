import { Component } from '@angular/core';
import { Member } from '../../_models/Members';
import { MembersService } from '../../_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css',
})
export class MemberListComponent {
  members: Member[];

  constructor(private memberService: MembersService) {}
  loadMembers() {
    this.memberService.getMembers().subscribe((members) => {
      this.members = members;
    });
  }
  ngOnInit(): void {
    this.loadMembers();
  }
}
