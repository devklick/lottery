import axios, { AxiosInstance, AxiosResponse } from "axios";

import {
  ApiMessages,
  ApiErrorsResponse,
  apiErrorsResponseSchema,
  ApiSuccessResponse,
  apiSuccessResponseSchema,
  apiMessagesSchema,
} from "../common/schemas";

export function isApiError(value: unknown): value is ApiErrorsResponse {
  return apiErrorsResponseSchema.safeParse(value).success;
}

export function isApiSuccess(value: unknown): value is ApiSuccessResponse {
  return apiSuccessResponseSchema.safeParse(value).success;
}

export type SuccessResult<T> = {
  success: true;
  data: T;
};

export type ErrorResult<T> = {
  success: false;
  errors: T;
};

export function isReAuthError<T>(result: ErrorResult<T>): boolean {
  const validation = apiMessagesSchema.safeParse(result.errors);
  return validation.success && validation.data[0].code === "RecentAuthRequired";
}

type Result<SuccessData> =
  | SuccessResult<SuccessData>
  | ErrorResult<ApiMessages>;

type AsyncResult<SuccessData> = Promise<Result<SuccessData>>;

type StatusCodeHandler = (response: AxiosResponse) => void;

type PostOptions = Partial<{
  withCredentials: boolean;
  onStatusCode: Record<number, StatusCodeHandler>;
}>;

type PutOptions = Partial<{
  withCredentials: boolean;
  onStatusCode: Record<number, StatusCodeHandler>;
}>;

type GetOptions = Partial<{
  onStatusCode: Record<number, StatusCodeHandler>;
  withCredentials: boolean;
}>;

export interface ApiServiceDefinition {
  post<Request = unknown, Response = unknown>(
    url: string,
    request?: Request,
    options?: PostOptions,
  ): AsyncResult<Response>;

  delete<Request = unknown, Response = unknown>(
    url: string,
    request?: Request,
    options?: PostOptions,
  ): AsyncResult<Response>;

  get<Query, Response>(
    url: string,
    query?: Query,
    options?: GetOptions,
  ): AsyncResult<Response>;

  put<Request = unknown, Response = unknown>(
    url: string,
    request?: Request,
    options?: PutOptions,
  ): AsyncResult<Response>;
}

interface ApiServiceParams {
  baseUrl: string;
}

export class ApiService implements ApiServiceDefinition {
  private readonly api: AxiosInstance;
  constructor(params: ApiServiceParams) {
    this.api = axios.create({
      baseURL: params.baseUrl,
      validateStatus: null,
    });
  }
  async delete<Request = unknown, Response = unknown>(
    url: string,
    request?: Request,
    options?: PostOptions,
  ): AsyncResult<Response> {
    console.info("Calling API", { url, request, options });
    const response = await this.api.delete<
      Response,
      AxiosResponse<Response>,
      Request
    >(url, {
      withCredentials: options?.withCredentials,
    });

    console.log("response", response);

    options?.onStatusCode?.[response.status]?.(response);

    if (
      response.status.toString().startsWith("2") &&
      isApiSuccess(response.data)
    ) {
      return {
        success: true,
        data: response.data.value,
      };
    }

    console.log("api response data", response.data);
    return {
      success: false,
      errors: isApiError(response.data)
        ? response.data.messages
        : [{ value: "Unknown error info received", code: "General" }],
    };
  }

  async post<Request = unknown, Response = unknown>(
    url: string,
    request?: Request,
    options?: PostOptions,
  ): AsyncResult<Response> {
    console.info("Calling API", { url, request, options });
    const response = await this.api.post<
      Response,
      AxiosResponse<Response>,
      Request
    >(url, request, {
      withCredentials: options?.withCredentials,
    });

    options?.onStatusCode?.[response.status]?.(response);

    if (
      response.status.toString().startsWith("2") &&
      isApiSuccess(response.data)
    ) {
      return {
        success: true,
        data: response.data.value,
      };
    }

    return {
      success: false,
      errors: isApiError(response.data)
        ? response.data.messages
        : [{ value: "Unknown error info received", code: "General" }],
    };
  }

  async put<Request = unknown, Response = unknown>(
    url: string,
    request?: Request | undefined,
    options?: PutOptions,
  ): AsyncResult<Response> {
    const response = await this.api.put<
      Response,
      AxiosResponse<Response>,
      Request
    >(url, request, {
      withCredentials: options?.withCredentials,
    });

    options?.onStatusCode?.[response.status]?.(response);

    if (
      response.status.toString().startsWith("2") &&
      isApiSuccess(response.data)
    ) {
      return {
        success: true,
        data: response.data.value,
      };
    }

    return {
      success: false,
      errors: isApiError(response.data)
        ? response.data.messages
        : [{ value: "Unknown error info received", code: "General" }],
    };
  }

  async get<Query, Response>(
    url: string,
    query?: Query,
    options?: GetOptions,
  ): AsyncResult<Response> {
    const response = await this.api.get<Response, AxiosResponse<Response>>(
      url,
      {
        params: query,
        withCredentials: options?.withCredentials,
        paramsSerializer: {
          indexes: true,
        },
      },
    );

    options?.onStatusCode?.[response.status]?.(response);

    if (
      response.status.toString().startsWith("2") &&
      isApiSuccess(response.data)
    ) {
      return {
        success: true,
        data: response.data.value,
      };
    }

    return {
      success: false,
      errors: isApiError(response.data)
        ? response.data.messages
        : [{ value: "Unknown error info received", code: "General" }],
    };
  }
}

export function createApiService(params: ApiServiceParams): ApiService {
  return new ApiService(params);
}
