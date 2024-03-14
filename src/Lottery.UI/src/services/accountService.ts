import { AxiosInstance, AxiosResponse } from "axios";
import {
  SignUpRequest,
  SignUpResponse,
} from "../pages/Account/SignUp/signUp.schema";
import lotteryApi, { post } from "./lotteryApi";
import {
  SignInRequest,
  SignInResponse,
} from "../pages/Account/SignIn/signIn.schema";

interface AccountService {
  signIn(request: SignInRequest): Promise<SignInResponse>;
  signUp(request: SignUpRequest): Promise<SignUpResponse>;
}

export function createAccountService({
  api,
}: {
  api: AxiosInstance;
}): AccountService {
  const signUp: AccountService["signUp"] = async (request) => {
    const result = await post<SignUpRequest, SignUpResponse>({
      api,
      request,
      url: "/account/signUp",
    });

    return result;
  };

  const signIn: AccountService["signIn"] = async (request) => {
    const result = await post<SignInRequest, SignInResponse>({
      api,
      request,
      url: "/account/signIn",
    });
    return result;
  };

  return { signIn, signUp };
}

export default createAccountService({ api: lotteryApi });
