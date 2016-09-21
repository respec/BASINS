#!/usr/bin/perl
require "cgi-lib.pl";
my $log_filename = "update.log";

&ReadParse;

#Read variables from the request
my $app = $in{"app"};
my $version = $in{"version"};
my $release = $in{"release"};

my $filename = "";
my $contenttype = "Content-type: text/plain\n\n";

open(LOG_FILE,">>$log_filename");

#put current date/time and IP address of requester in log
($sec,$min,$hour,$mday,$mon,$year,$wday,$yday,$isdst)=localtime(time);
printf LOG_FILE "%4d-%02d-%02d %02d:%02d:%02d %s ",$year+1900,$mon+1,$mday,$hour,$min,$sec,$ENV{REMOTE_ADDR};

print LOG_FILE "$app $version $release";

if ($app eq "BASINS") {

    if ($version eq "3.1") {

	if ($release eq "6") {
	    $filename = "Update.xml";
	}
	else { # release > 6
            print $contenttype;
            print "No update available for $app $version $release";
        }
    }
}

#app != BASINS

elsif ($app eq "test")    { $filename = "update-test.xml" }
elsif ($app eq "bigtest") { $filename = "update-bigtest.xml" }
else {
    print $contenttype;
    print "No updates available for $app $version $release"
}

unless ($filename eq "") {
    #reply with an XML file
    my @reply;
    print LOG_FILE " sending $filename";
    print $contenttype;
    open (XML, $filename);
    @reply = <XML>;
    print @reply;
    close XML;
}

print LOG_FILE "\n";
close(LOG_FILE);
