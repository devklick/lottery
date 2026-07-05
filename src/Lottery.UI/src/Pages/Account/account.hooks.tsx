import { useMutation, useQuery } from "@tanstack/react-query";
import { useEffect } from "react";

import { UpdateAccountResponse } from "./AccountDetails/schema";
import accountService from "./accountService";
import { GetAccountResponse } from "./SignUp/getAccount.schema";
import { notifyInfo, notifySuccess } from "../../common/notifications";
import { ErrorResult, isReAuthError } from "../../services/ApiService";

interface UseGetAccountProps {
  enabled: boolean;
  onSuccess(data: GetAccountResponse): void;
}

export function useGetAccount({ enabled, onSuccess }: UseGetAccountProps) {
  const query = useQuery({
    queryKey: ["account", "get"],
    queryFn: () => accountService.getAccount(),
    enabled,
  });
  useEffect(() => {
    console.log("hook runnning");
    if (query.status === "success" && query.data) {
      onSuccess(query.data);
    }
  }, [onSuccess, query.data, query.status]);

  return query;
}

interface UseUpdateAccountProps {
  onSuccess(data: UpdateAccountResponse): void;
  onReAuthRequired(): void;
}

export function useUpdateAccount({
  onSuccess,
  onReAuthRequired,
}: UseUpdateAccountProps) {
  return useMutation({
    mutationFn: accountService.updateAccount,
    onSuccess(data) {
      notifySuccess({
        title: "Account details updated",
        message: "Your new details have been saved",
      });
      if (data.email.messages?.[0].code === "EmailVerificationRequired") {
        notifyInfo({
          title: "Verification email send",
          message:
            "Please check your emails to confirm the new email address. Once confirmed, it will be updated",
        });
      }
      onSuccess(data);
    },
    onError: (e: ErrorResult<unknown>) => {
      if (isReAuthError(e)) onReAuthRequired();
    },
  });
}

interface UseConfirmPasswordProps {
  onSuccess(): void;
  onFailure(): void;
}

export function useConfirmPassword({
  onFailure,
  onSuccess,
}: UseConfirmPasswordProps) {
  return useMutation({
    mutationFn: accountService.confirmPassword,
    onSuccess,
    onError: onFailure,
  });
}
