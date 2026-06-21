import {
  Button,
  Checkbox,
  Group,
  InputLabel,
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
import AnchorLink from "../../components/AnchorLink/AnchorLink";
import Page from "../../components/Page";
import PageSection from "../../components/PageSection";
import { useUserStore } from "../../stores/user.store";
import accountService from "../accountService";

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface SignInProps {}

// eslint-disable-next-line no-empty-pattern
function SignIn({}: SignInProps) {
  const initialValues: SignInRequest = {
    password: "",
    username: "",
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

  const mutation = useMutation<SignInResponse, unknown, SignInRequest>({
    mutationFn: async (request) => await accountService.signIn(request),
    onSuccess: onSignInSuccess,
  });

  return (
    <Page title={{ value: "Sign In", align: "center" }}>
      <PageSection width={"auto"}>
        <form onSubmit={form.onSubmit((data) => mutation.mutate(data))}>
          <Stack gap={24}>
            <TextInput
              {...form.getInputProps("username")}
              placeholder="Username"
              name="username"
              type="text"
            />

            <PasswordInput
              {...form.getInputProps("password")}
              placeholder="Password"
              name="password"
              type="password"
            />

            <Group justify="space-between">
              <Checkbox
                {...form.getInputProps("staySignedIn")}
                defaultChecked={initialValues.staySignedIn}
                name="stay-signed-in"
              />
              <InputLabel fw={"normal"}>Stay signed in</InputLabel>
            </Group>

            <Button variant="filled" type="submit">
              Submit
            </Button>
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
