import axios, { AxiosInstance, AxiosResponse } from "axios";

export default axios.create({ baseURL: "/lotteryapi" });

type PostParams<Request> = {
  api: AxiosInstance;
  url: string;
  request: Request;
};
export async function post<Request, Response>({
  api,
  request,
  url,
}: PostParams<Request>): Promise<AxiosResponse<Response>> {
  return await api.post<Response, AxiosResponse<Response>, Request>(
    url,
    request,
    { withCredentials: true }
  );
}
