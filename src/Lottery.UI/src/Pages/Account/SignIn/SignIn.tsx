import {
  Button,
  Checkbox,
  PasswordInput,
  Stack,
  Text,
  TextInput,
} from "@mantine/core";
import { schemaResolver, useForm } from "@mantine/form";
import { useMutation } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";

import {
  SignInRequest,
  SignInResponse,
  signInRequestSchema,
} from "./signIn.schema";
import { notifyError } from "../../../common/notifications";
import AnchorLink from "../../../components/AnchorLink/AnchorLink";
import FieldWrapper from "../../../components/FieldWrapper";
import Page from "../../../components/Page";
import PageSection from "../../../components/PageSection";
import { useUserStore } from "../../../stores/user.store";
import accountService from "../accountService";

function SignIn() {
  const initialValues: SignInRequest = {
    password: "",
    usernameOrEmail: "",
    staySignedIn: true,
  };
  const form = useForm<SignInRequest>({
    validate: schemaResolver(signInRequestSchema),
    initialValues,
    validateInputOnChange: true,
  });

  const navigate = useNavigate();
  const userStore = useUserStore();

  function onSignInSuccess(response: SignInResponse) {
    userStore.login(response.userType, response.sessionExpiry);
    navigate("/home");
  }

  function onSignInFailed() {
    notifyError({
      title: "Login failed",
      message: "Please check your credentials and try again",
    });
  }

  const mutation = useMutation<SignInResponse, unknown, SignInRequest>({
    mutationFn: async (request) => await accountService.signIn(request),
    onSuccess: onSignInSuccess,
    onError: onSignInFailed, // todo: properly handle all failed login scenarios
  });

  return (
    <Page title={{ value: "Sign In", align: "center" }}>
      <PageSection width={"auto"}>
        <form onSubmit={form.onSubmit((data) => mutation.mutate(data))}>
          <Stack gap={24}>
            <FieldWrapper
              name="Username or email"
              required
              description={
                signInRequestSchema.shape.usernameOrEmail.description
              }
            >
              <TextInput
                {...form.getInputProps("usernameOrEmail")}
                placeholder="Username or Email"
                name="username-or-email"
                type="text"
              />
            </FieldWrapper>

            <FieldWrapper
              name="Password"
              required
              description={signInRequestSchema.shape.password.description}
            >
              <PasswordInput
                {...form.getInputProps("password")}
                placeholder="Password"
                name="password"
                type="password"
              />
            </FieldWrapper>

            <FieldWrapper
              name="Stay signed in"
              direction="row"
              description={signInRequestSchema.shape.staySignedIn.description}
              fieldFlex={"0 1 0"}
            >
              <Checkbox
                {...form.getInputProps("staySignedIn")}
                defaultChecked={initialValues.staySignedIn}
                name="stay-signed-in"
              />
            </FieldWrapper>

            <Button variant="filled" type="submit">
              Submit
            </Button>
            <Text size="sm">
              <AnchorLink to="/account/password/forgot">
                Forgot your password?
              </AnchorLink>
            </Text>
            <Text size="sm">
              Don't have an account?{" "}
              <AnchorLink to="/account/signUp">Sign up</AnchorLink>
            </Text>
          </Stack>
        </form>
      </PageSection>
    </Page>
  );
}

export default SignIn;
