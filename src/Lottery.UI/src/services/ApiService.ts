import axios, { AxiosInstance, AxiosResponse } from "axios";

export function isBasicError(value: unknown): value is BasicError {
  return !!value && typeof value === "object" && "errors" in value;
}

type BasicError = {
  errors: Array<{ message: string }>;
};

type SuccessResult<T> = {
  success: true;
  data: T;
};

type ErrorResult<T> = {
  success: false;
  error: T;
};

type Result<SuccessData> = SuccessResult<SuccessData> | ErrorResult<BasicError>;

type AsyncResult<SuccessData> = Promise<Result<SuccessData>>;

type StatusCodeHandler = () => void;

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
    options?: PostOptions
  ): AsyncResult<Response>;

  get<Query, Response>(
    url: string,
    query?: Query,
    options?: GetOptions
  ): AsyncResult<Response>;

  put<Request = unknown, Response = unknown>(
    url: string,
    request?: Request,
    options?: PutOptions
  ): AsyncResult<Response>;
}

interface ApiServiceParams {
  baseUrl: string;
}

export class ApiService implements ApiServiceDefinition {
  private readonly api: AxiosInstance;
  constructor(params: ApiServiceParams) {
    this.api = axios.create({ baseURL: params.baseUrl });
  }

  async post<Request = unknown, Response = unknown>(
    url: string,
    request?: Request,
    options?: PostOptions
  ): AsyncResult<Response> {
    const response = await this.api.post<
      Response,
      AxiosResponse<Response | BasicError>,
      Request
    >(url, request, {
      withCredentials: options?.withCredentials,
    });

    options?.onStatusCode && options.onStatusCode[response.status]?.();

    if (
      response.status.toString().startsWith("2") &&
      !isBasicError(response.data)
    ) {
      return {
        success: true,
        data: response.data,
      };
    }

    return {
      success: false,
      error: isBasicError(response.data)
        ? response.data
        : { errors: [{ message: "Unknown error info received" }] },
    };
  }

  async put<Request = unknown, Response = unknown>(
    url: string,
    request?: Request | undefined,
    options?: PutOptions
  ): AsyncResult<Response> {
    const response = await this.api.put<
      Response,
      AxiosResponse<Response | BasicError>,
      Request
    >(url, request, {
      withCredentials: options?.withCredentials,
    });

    options?.onStatusCode && options.onStatusCode[response.status]?.();

    if (
      response.status.toString().startsWith("2") &&
      !isBasicError(response.data)
    ) {
      return {
        success: true,
        data: response.data,
      };
    }

    return {
      success: false,
      error: isBasicError(response.data)
        ? response.data
        : { errors: [{ message: "Unknown error info received" }] },
    };
  }

  async get<Query, Response>(
    url: string,
    query?: Query,
    options?: GetOptions
  ): AsyncResult<Response> {
    const response = await this.api.get<Response, AxiosResponse<Response>>(
      url,
      {
        params: query,
        withCredentials: options?.withCredentials,
        paramsSerializer: {
          indexes: true,
        },
      }
    );

    options?.onStatusCode && options.onStatusCode[response.status]?.();

    if (
      response.status.toString().startsWith("2") &&
      !isBasicError(response.data)
    ) {
      return {
        success: true,
        data: response.data,
      };
    }

    return {
      success: false,
      error: isBasicError(response.data)
        ? response.data
        : { errors: [{ message: "Unknown error info received" }] },
    };
  }
}

export function createApiService(params: ApiServiceParams): ApiService {
  return new ApiService(params);
}
