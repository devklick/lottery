import { ApiServiceDefinition, createApiService } from "../services/ApiService";
import {
  AcceptInviteRequest,
  AcceptInviteRequestBody,
  AcceptInviteResponse,
  VerifyInviteRequestQuery,
  VerifyInviteResponse,
  acceptInviteResponseSchema,
  verifyInviteResponseSchema,
} from "./Invite/Accept/accept.schema";
import {
  UserInviteRequest,
  UserInviteRequestBody,
  UserInviteResponse,
  userInviteResponseSchema,
} from "./Invite/invite.schema";

interface UserService {
  inviteUser: (request: UserInviteRequest) => Promise<UserInviteResponse>;
  verifyInvite: (
    request: VerifyInviteRequestQuery,
  ) => Promise<VerifyInviteResponse>;
  acceptInvite: (request: AcceptInviteRequest) => Promise<AcceptInviteResponse>;
}

export function createUserService({
  api,
}: {
  api: ApiServiceDefinition;
}): UserService {
  const inviteUser: UserService["inviteUser"] = async (request) => {
    const result = await api.post<UserInviteRequestBody, UserInviteResponse>(
      "/user/invite",
      request.body,
      { withCredentials: true },
    );

    if (!result.success) {
      throw result.errors;
    }

    const valid = userInviteResponseSchema.safeParse(result.data);

    if (valid.success) {
      return valid.data;
    }

    throw valid.error.message;
  };

  const verifyInvite: UserService["verifyInvite"] = async (request) => {
    const result = await api.get<VerifyInviteRequestQuery, UserInviteResponse>(
      "/user/invite/verify",
      request,
    );

    if (!result.success) {
      throw result.errors;
    }

    const valid = verifyInviteResponseSchema.safeParse(result.data);

    if (valid.success) {
      return valid.data;
    }

    throw valid.error.message;
  };

  const acceptInvite: UserService["acceptInvite"] = async (request) => {
    const result = await api.post<
      AcceptInviteRequestBody,
      AcceptInviteResponse
    >("/user/invite/accept", request.body);

    if (!result.success) {
      throw result.errors;
    }

    const valid = acceptInviteResponseSchema.safeParse(result.data || {});

    if (valid.success) {
      return valid.data;
    }

    throw valid.error.message;
  };

  return { inviteUser, verifyInvite, acceptInvite };
}

export default createUserService({
  api: createApiService({ baseUrl: "/lotteryapi" }),
});
