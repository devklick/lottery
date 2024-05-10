import { ApiServiceDefinition, createApiService } from "../services/ApiService";
import {
  UserInviteRequest,
  UserInviteRequestBody,
  UserInviteResponse,
  userInviteResponseSchema,
} from "./UserInvite/userInvite.schema";

interface UserService {
  inviteUser: (request: UserInviteRequest) => Promise<UserInviteResponse>;
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
      { withCredentials: true }
    );

    if (!result.success) {
      throw result.error;
    }

    const valid = userInviteResponseSchema.safeParse(result.data);

    if (valid.success) {
      return valid.data;
    }

    throw valid.error.message;
  };

  return { inviteUser };
}

export default createUserService({
  api: createApiService({ baseUrl: "/lotteryapi" }),
});
