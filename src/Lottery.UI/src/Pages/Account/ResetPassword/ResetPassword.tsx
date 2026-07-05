import { Button, PasswordInput, Stack, TextInput } from "@mantine/core";
import { schemaResolver, useForm } from "@mantine/form";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { useResetPassword } from "./hooks";
import {
  ResetPasswordForm,
  resetPasswordFormSchema,
  resetPasswordPageQuerySchema,
} from "./schema";
import { notifySuccess } from "../../../common/notifications";
import Page from "../../../components/Page";
import PageSection from "../../../components/PageSection";
import useValidatedQueryParams from "../../../hooks/url/useValidatedSearchParams";

export default function ResetPassword() {
  const resetPassword = useResetPassword({ onSuccess: onResetPasswordSuccess });
  const query = useValidatedQueryParams(resetPasswordPageQuerySchema);
  const navigate = useNavigate();

  const form = useForm<ResetPasswordForm>({
    validate: schemaResolver(resetPasswordFormSchema),
    validateInputOnChange: true,
  });

  useEffect(() => {
    if (!query.success) {
      navigate("/home");
    }
  }, [navigate, query.success]);

  function onResetPasswordSuccess() {
    notifySuccess({
      title: "Password Updated!",
      message: "You can now log in using your new password",
    });
    navigate("/account/signIn");
  }

  if (!query.success) return;

  return (
    <Page title={{ value: "Reset Password", align: "center" }}>
      <PageSection width={"auto"}>
        <form
          onSubmit={form.onSubmit((data) =>
            resetPassword.mutate({ ...data, token: query.data.token }),
          )}
        >
          <Stack gap={24}>
            <TextInput
              {...form.getInputProps("email")}
              placeholder="Email"
              name="email"
              type="email"
            />

            <PasswordInput
              {...form.getInputProps("password")}
              placeholder="Password"
              name="password"
              type="password"
            />
            <Button variant="filled" type="submit">
              Submit
            </Button>
          </Stack>
        </form>
      </PageSection>
    </Page>
  );
}
