import { schemaResolver, useForm } from "@mantine/form";
import {
  SignUpRequest,
  SignUpResponse,
  signUpRequestSchema,
} from "./signUp.schema";
import {
  Anchor,
  Button,
  Container,
  Paper,
  PasswordInput,
  Stack,
  Text,
  TextInput,
  Title,
} from "@mantine/core";
import { useMutation } from "@tanstack/react-query";
import accountService from "../accountService";
import { useNavigate } from "react-router-dom";
import Page from "../../components/Page";
import PageSection from "../../components/PageSection";
import AnchorLink from "../../components/AnchorLink/AnchorLink";

interface SignUpProps {}

function SignUp({}: SignUpProps) {
  const form = useForm<SignUpRequest>({
    validate: schemaResolver(signUpRequestSchema),
    validateInputOnChange: true,
  });

  const navigate = useNavigate();

  const mutation = useMutation<SignUpResponse, unknown, SignUpRequest>({
    mutationFn: async (request) => await accountService.signUp(request),
    onSuccess: () => navigate("/home"),
  });

  return (
    <Page title={{ value: "Sign Up", align: "center" }}>
      <PageSection width={"auto"}>
        <form onSubmit={form.onSubmit((data) => mutation.mutate(data))}>
          <Stack gap={24}>
            <TextInput placeholder="Email" {...form.getInputProps("email")} />

            <TextInput
              placeholder="Username"
              {...form.getInputProps("username")}
            />

            <PasswordInput
              placeholder="Password"
              {...form.getInputProps("password")}
            />

            <PasswordInput
              placeholder="Confirm Password"
              {...form.getInputProps("confirmPassword")}
            />

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
