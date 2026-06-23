import { skipToken, useQuery } from "@tanstack/react-query";

import { ConfirmEmailChangeRequestQuery } from "./schema";
import accountService from "../accountService";

export function useConfirmEmailChange(
  request: ConfirmEmailChangeRequestQuery | undefined,
) {
  return useQuery({
    queryKey: [
      "confirm",
      "email",
      "change",
      request?.token,
      request?.userId,
      request?.email,
    ],
    queryFn: request
      ? () => accountService.confirmEmailChange(request)
      : skipToken,
    enabled: !!request,
  });
}
