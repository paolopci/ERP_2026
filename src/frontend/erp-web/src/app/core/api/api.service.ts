import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

type ApiEnvelope<T> = {
  data: T;
  errors: { code: string; message: string; target?: string }[];
  meta: Record<string, unknown>;
  traceId: string;
};

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/v1';

  getInventorySummary(): Observable<ApiEnvelope<{ message: string; timestampUtc: string }>> {
    return this.http.get<ApiEnvelope<{ message: string; timestampUtc: string }>>(`${this.baseUrl}/inventory/summary`);
  }
}
