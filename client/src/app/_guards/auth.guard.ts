import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accountService:AccountService,private toastr:ToastrService, private router: Router){}
  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(user =>
        {
        if(user) return true;
        else
        {
        this.toastr.error('you shall not pass')
        return false;
        }
      })
    )
  }

//   canActivate(route: ActivatedRouteSnapshot): any {
//     const user = localStorage.getItem('user');
//     console.log('uid', localStorage.getItem('user'));
//     if (user && user != null && user !== 'null') {
//         return true;
//     }

    
//     this.router.navigate(['/login']);
//     return false;
// }
}
