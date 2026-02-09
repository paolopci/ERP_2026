import { HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export const traceIdInterceptor: HttpInterceptorFn = (req, next) => {
  const traceId = crypto.randomUUID();
  const enrichedRequest = req.clone({
    setHeaders: { 'x-trace-id': traceId }
  });

  return next(enrichedRequest).pipe(
    catchError((error) => {
      if (error?.error?.traceId) {
        // eslint-disable-next-line no-console
        console.error(`API error traceId: ${error.error.traceId}`);
      }

      return throwError(() => error);
    })
  );
};
