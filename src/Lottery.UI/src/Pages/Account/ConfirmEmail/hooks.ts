import { skipToken, useQuery } from "@tanstack/react-query";

import { ConfirmEmailRequestQuery } from "./schema";
import accountService from "../accountService";

export function useConfirmEmail(request: ConfirmEmailRequestQuery | undefined) {
  return useQuery({
    queryKey: ["confirm", "email", request?.token, request?.userId],
    queryFn: request ? () => accountService.confirmEmail(request) : skipToken,
    enabled: !!request,
  });
}
