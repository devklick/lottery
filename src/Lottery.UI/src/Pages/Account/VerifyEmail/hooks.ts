import { skipToken, useQuery } from "@tanstack/react-query";

import { VerifyEmailRequestQuery } from "./schema";
import accountService from "../accountService";

export function useVerifyEmail(request: VerifyEmailRequestQuery | undefined) {
  return useQuery({
    queryKey: ["verify", "email", request?.token, request?.userId],
    queryFn: request ? () => accountService.verifyEmail(request) : skipToken,
    enabled: !!request,
  });
}
