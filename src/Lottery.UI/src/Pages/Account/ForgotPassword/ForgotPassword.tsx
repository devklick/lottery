import { Button, Stack, TextInput } from "@mantine/core";
import { schemaResolver, useForm } from "@mantine/form";
import { notifications } from "@mantine/notifications";
import { IconCircleCheck } from "@tabler/icons-react";

import { useForgotPassword } from "./hooks";
import {
  ForgotPasswordRequestBody,
  forgotPasswordRequestBodySchema,
} from "./schema";
import Page from "../../../components/Page";
import PageSection from "../../../components/PageSection";

export default function ForgotPassword() {
  const forgotPassword = useForgotPassword({
    onSuccess: handleSuccess,
  });

  const form = useForm<ForgotPasswordRequestBody>({
    validate: schemaResolver(forgotPasswordRequestBodySchema),
    validateInputOnChange: true,
  });

  function handleSuccess() {
    notifications.show({
      title: "Request submitted",
      message:
        "If your email is registered, a password reset link will be sent",
      icon: <IconCircleCheck />,
      color: "green",
    });
  }

  return (
    <Page title={{ value: "Forgot Password", align: "center" }}>
      <PageSection
        width={"auto"}
        subheader="Enter your email address and we'll send you a link to reset your password"
      >
        <form onSubmit={form.onSubmit((data) => forgotPassword.mutate(data))}>
          <Stack gap={24} maw={300}>
            <TextInput
              {...form.getInputProps("email")}
              placeholder="Email"
              name="email"
              type="email"
            />

            <Button variant="filled" type="submit" disabled={!form.isValid()}>
              Submit
            </Button>
          </Stack>
        </form>
      </PageSection>
    </Page>
  );
}
