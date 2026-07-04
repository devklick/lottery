import { Button, PasswordInput, Stack, Text, TextInput } from "@mantine/core";
import { schemaResolver, useForm } from "@mantine/form";
import { notifications } from "@mantine/notifications";
import { IconCircleCheck } from "@tabler/icons-react";
import { useMutation } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";

import {
  SignUpRequest,
  SignUpResponse,
  signUpRequestSchema,
} from "./signUp.schema";
import AnchorLink from "../../../components/AnchorLink/AnchorLink";
import FieldWrapper from "../../../components/FieldWrapper";
import Page from "../../../components/Page";
import PageSection from "../../../components/PageSection";
import accountService from "../accountService";

function SignUp() {
  const form = useForm<SignUpRequest>({
    validate: schemaResolver(signUpRequestSchema),
    validateInputOnChange: true,
  });

  const navigate = useNavigate();

  const mutation = useMutation<SignUpResponse, unknown, SignUpRequest>({
    mutationFn: accountService.signUp,
    onSuccess: () => {
      notifications.show({
        title: "Account Created",
        message: "You can now log into your account",
        icon: <IconCircleCheck />,
        color: "green",
      });
      navigate("/account/signIn");
    },
  });

  return (
    <Page title={{ value: "Sign Up", align: "center" }}>
      <PageSection width={"auto"}>
        <form onSubmit={form.onSubmit((data) => mutation.mutate(data))}>
          <Stack gap={24}>
            <FieldWrapper
              name="Email"
              description={signUpRequestSchema.shape.email.description}
              required
            >
              <TextInput
                {...form.getInputProps("email")}
                placeholder="Email"
                name="email"
                type="email"
              />
            </FieldWrapper>

            <FieldWrapper
              name="Username"
              description={signUpRequestSchema.shape.username.description}
              required
            >
              <TextInput
                {...form.getInputProps("username")}
                placeholder="Username"
                name="username"
                type="text"
              />
            </FieldWrapper>

            <FieldWrapper
              name="Password"
              description={signUpRequestSchema.shape.password.description}
              required
            >
              <PasswordInput
                {...form.getInputProps("password")}
                placeholder="Password"
                name="password"
                type="password"
              />
            </FieldWrapper>

            <FieldWrapper
              name="Confirm Password"
              description={
                signUpRequestSchema.shape.confirmPassword.description
              }
              required
            >
              <PasswordInput
                {...form.getInputProps("confirmPassword")}
                placeholder="Confirm Password"
                name="confirm-password"
                type="password"
              />
            </FieldWrapper>

            <Button variant="filled" type="submit">
              Submit
            </Button>

            <Text size="sm">
              Already have an account?{" "}
              <AnchorLink to="/account/signIn">Sign in</AnchorLink>
            </Text>
          </Stack>
        </form>
      </PageSection>
    </Page>
  );
}

export default SignUp;
